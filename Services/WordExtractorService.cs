using AnnotationsExtractor.Models;
using AnnotationsExtractor.Resources;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace AnnotationsExtractor.Services;

public sealed class WordExtractorService
{
    private Report ExtractCore(string filePath, IProgress<OperationProgress>? progress)
    {
        dynamic? word = null;
        dynamic? document = null;
        Report? report = null;

        try
        {
            progress?.Report(new OperationProgress(2, Strings.StartingWordReader));
            Type? wordType = Type.GetTypeFromProgID(Strings.WordApplicationIdentifier);

            if (wordType is null)
            {
                throw new InvalidOperationException(Strings.WordErrorNotFound);
            }

            word = Activator.CreateInstance(wordType);

            if (word is null)
            {
                throw new InvalidOperationException(Strings.WordErrorCannotStart);
            }

            ConfigureWord(word);

            progress?.Report(new OperationProgress(8,Strings.OpeningReport));
            document = word.Documents.Open(filePath, ConfirmConversions: false, ReadOnly: true, AddToRecentFiles: false, Visible: false);
            progress?.Report(new OperationProgress(12, Strings.AnalyzingReport));
            report = ReadReport(document, Path.GetFileName(filePath), progress);
        }
        finally
        {
            progress?.Report(new OperationProgress(98, Strings.ClosingDocument));
            CloseDocument(document);
            QuitWord(word);
        }

        progress?.Report(new OperationProgress(100, Strings.ReadCompleted));
        return report ?? throw new InvalidDataException(Strings.ReportReadError);
    }

    public Task<Report> ExtractAsync(string filePath, IProgress<OperationProgress>? progress = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(Strings.FileNotFoundError, filePath);
        }

        TaskCompletionSource<Report> completionSource =new(TaskCreationOptions.RunContinuationsAsynchronously);

        Thread thread = new(
            () =>
            {
                try
                {
                    Report report = ExtractCore(filePath, progress);
                    completionSource.SetResult(report);
                }
                catch (Exception ex)
                {
                    completionSource.SetException(ex);
                }
            }
        )
        {
            IsBackground = true
        };

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();

        return completionSource.Task;
    }

    private static Report ReadReport(dynamic document,string fileName,IProgress<OperationProgress>? progress)
    {
        string? course = null;
        List<StudentBuilder> studentBuilders = [];
        StudentBuilder? currentStudent = null;
        int tableCount = document.Tables.Count;

        for (int tableIndex = 1; tableIndex <= tableCount; tableIndex++)
        {
            dynamic table = document.Tables[tableIndex];

            HeaderData? header = TryReadHeader(table);

            if (header is not null)
            {
                if (!string.IsNullOrWhiteSpace(header.Course))
                {
                    course ??= header.Course;
                }

                if (currentStudent is null || !currentStudent.Name.Equals(header.Student, StringComparison.OrdinalIgnoreCase))
                {
                    currentStudent = new StudentBuilder(header.Student);
                    studentBuilders.Add(currentStudent);
                }
            }

            List<Annotation> annotations = ExtractAnnotationsFromTable(table);

            if (annotations.Count > 0)
            {
                if (currentStudent is null)
                {
                    throw new InvalidDataException(Strings.UnkownStudentError);
                }

                currentStudent.Annotations.AddRange(annotations);
            }

            int percentage = 12 + (int)Math.Round((double)tableIndex / tableCount * 84);

            progress?.Report(new OperationProgress(percentage, string.Format(Strings.ProcessingReportStructure, tableIndex, tableCount))); 
        }

        if (string.IsNullOrWhiteSpace(course))
        {
            throw new InvalidDataException(Strings.UnkownCourseError);
        }

        if (studentBuilders.Count == 0)
        {
            throw new InvalidDataException(Strings.EmptyReportError);
        }

        List<Student> students = [];

        foreach (StudentBuilder builder in studentBuilders)
        {
            students.Add(new Student{Name = builder.Name, Annotations = builder.Annotations.ToArray()});
        }

        return new Report
        {
            FileName = fileName,
            Course = course,
            Students = students
        };
    }

    private static HeaderData? TryReadHeader(dynamic table)
    {
        int cellCount = table.Range.Cells.Count;

        for (int cellIndex = 1; cellIndex <= cellCount; cellIndex++)
        {
            dynamic cell = table.Range.Cells[cellIndex];

            string text = CleanCellText((string)cell.Range.Text);

            if (!text.Contains(Strings.HeaderDataStudentTitle, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string student = ExtractStudent(text);
            string? course = TryExtractCourse(text);

            return new HeaderData(course, student);
        }

        return null;
    }

    private static List<Annotation> ExtractAnnotationsFromTable(dynamic outerTable)
    {
        List<Annotation> annotations = [];

        int cellCount =
            outerTable.Range.Cells.Count;

        for (int cellIndex = 1; cellIndex <= cellCount; cellIndex++)
        {
            dynamic outerCell = outerTable.Range.Cells[cellIndex];

            int nestedTableCount = outerCell.Tables.Count;

            for (int nestedIndex = 1; nestedIndex <= nestedTableCount; nestedIndex++)
            {
                dynamic nestedTable = outerCell.Tables[nestedIndex];

                if (!IsAnnotationsTable(nestedTable))
                {
                    continue;
                }

                annotations.AddRange(ReadAnnotationsTable(nestedTable));
            }
        }

        return annotations;
    }

    private static bool IsAnnotationsTable(dynamic table)
    {
        try
        {
            if (table.Rows.Count < 2)
            {
                return false;
            }

            string dateHeader = GetCellText(table, 2, 1);
            string enteredByHeader = GetCellText(table, 2, 2);
            string categoryHeader = GetCellText(table, 2, 3);
            string descriptionHeader = GetCellText(table, 2, 4);

            return
                dateHeader.Equals(Strings.HeaderDate, StringComparison.OrdinalIgnoreCase)
                &&
                enteredByHeader.Equals(Strings.HeaderWrittenBy, StringComparison.OrdinalIgnoreCase)
                &&
                categoryHeader.Equals(Strings.HeaderCategory, StringComparison.OrdinalIgnoreCase)
                &&
                descriptionHeader.Equals(Strings.HeaderDescription, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private static List<Annotation>
        ReadAnnotationsTable(dynamic table)
    {
        List<Annotation> annotations = [];

        int rowCount = table.Rows.Count;

        for (int rowIndex = 3; rowIndex <= rowCount; rowIndex++)
        {
            string dateText = GetCellText(table, rowIndex, 1);
            string enteredBy = GetCellText(table, rowIndex, 2);
            string category = GetCellText(table, rowIndex, 3);
            string description = GetCellText(table, rowIndex, 4);

            if (string.IsNullOrWhiteSpace(dateText))
            {
                continue;
            }

            if (!DateTime.TryParseExact(dateText,Strings.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,out DateTime date))
            {
                throw new InvalidDataException(String.Format(Strings.InvalidDateTimeFormatError, dateText, rowIndex));                
            }

            annotations.Add(
                new Annotation
                {
                    Date = date,
                    EnteredBy = enteredBy,
                    Category = category,
                    Description = description
                }
            );
        }

        return annotations;
    }

    private static string ExtractStudent(string headerText)
    {
        string studentTitle = Regex.Escape(Strings.HeaderDataStudentTitle);
        Match match = Regex.Match(
            headerText,
            $@"{studentTitle}\s*(?<value>[^\r\n]+)",
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant
        );

        if (!match.Success)
        {
            throw new InvalidDataException(Strings.ExtractingStudentNameError);
        }

        return match
            .Groups["value"]
            .Value
            .Trim();
    }

    private static string? TryExtractCourse(string headerText)
    {
        string courseTitle = Regex.Escape(Strings.HeaderCourseTitle);

        Match match = Regex.Match(
            headerText,
            $@"{courseTitle}\s*(?<{Strings.HeaderMatchGroup}>[^\r\n]+)",
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant
        );

        if (!match.Success)
        {
            return null;
        }

        return match.Groups[Strings.HeaderMatchGroup].Value.Trim();
    }

    private static string GetCellText(dynamic table, int row, int column)
    {
        string text = (string)table.Cell(row, column).Range.Text;
        return CleanCellText(text);
    }

    private static string CleanCellText(string text)
    {
        return text.TrimEnd('\r', '\a').Trim();
    }

    private static void ConfigureWord(dynamic word)
    {
        word.Visible = false;
        // wdAlertsNone
        word.DisplayAlerts = 0;
        try
        {
            // msoAutomationSecurityForceDisable
            word.AutomationSecurity = 3;
        }
        catch
        {
            // Ignorar si la versión de Office
            // no expone esta propiedad.
        }
    }

    private static void CloseDocument(dynamic? document)
    {
        if (document is null)
        {
            return;
        }

        try
        {
            document.Close(false);
        }
        catch
        {
            // Word puede haber cerrado el documento.
        }
    }

    private static void QuitWord(dynamic? word)
    {
        if (word is null)
        {
            return;
        }

        try
        {
            word.Quit(false);
        }
        catch
        {
            // Word puede haber terminado previamente.
        }
    }

    private sealed class StudentBuilder(string name)
    {
        public string Name { get; } = name;
        public List<Annotation> Annotations { get; } = [];
    }

    private sealed record HeaderData(string? Course, string Student);
}