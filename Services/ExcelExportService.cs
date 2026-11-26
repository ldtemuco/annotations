using AnnotationsExtractor.Models;
using AnnotationsExtractor.Resources;
using ClosedXML.Excel;
using System.IO;


namespace AnnotationsExtractor.Services;

public sealed class ExcelExportService
{
    private readonly ScheduleService _scheduleService;

    public ExcelExportService(ScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }
    private void ExportCore(Report report, string filePath, IProgress<OperationProgress>? progress)
    {
        string? directory = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        progress?.Report(new OperationProgress(2, Strings.PreparingWorkbook));

        using XLWorkbook workbook = new();
        int annotationCount = report.Students.Sum(student => student.Annotations.Count);
        int totalItems = annotationCount + report.Students.Count;
        int processedItems = 0;

        CreateAnnotationsWorksheet(workbook, report, progress, ref processedItems, totalItems);
        CreateSummaryWorksheet(workbook, report, progress, ref processedItems, totalItems);

        progress?.Report(new OperationProgress(95, Strings.SavingExcelFile));
        workbook.SaveAs(filePath);
        progress?.Report(new OperationProgress(100, Strings.ExportCompletedStatus));
    }

    public Task ExportAsync(Report report, string filePath, IProgress<OperationProgress>? progress = null)
    {
        ArgumentNullException.ThrowIfNull(report);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        return Task.Run(() => ExportCore(report,filePath,progress));
    }

    private void CreateAnnotationsWorksheet(XLWorkbook workbook, Report report, IProgress<OperationProgress>? progress, ref int processedItems, int totalItems)
    {
        IXLWorksheet worksheet = workbook.Worksheets.Add(Strings.ExcelWorksheetName);
        CourseInfo courseInfo = _scheduleService.GetCourseInfo(report.Course);

        const int headerRow = 4;
        const int firstDataRow = headerRow + 1;

        // -----------------------------------------------------
        // INFORMACIÓN DEL CURSO
        // -----------------------------------------------------

        worksheet.Cell(1, 1).Value = Strings.ExcelMetadataCourse;
        worksheet.Cell(1, 2).Value = courseInfo.Code;
        worksheet.Cell(2, 1).Value = Strings.ExcelMetadataHeadTeacher;
        worksheet.Cell(2, 2).Value = courseInfo.HeadTeacher;
        worksheet.Cell(3, 1).Value = Strings.ExcelMetadataSector;
        worksheet.Cell(3, 2).Value = courseInfo.Sector;
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(2, 1).Style.Font.Bold = true;
        worksheet.Cell(3, 1).Style.Font.Bold = true;
        worksheet.Range(1, 2, 1, 6).Merge();
        worksheet.Range(2, 2, 2, 6).Merge();
        worksheet.Range(3, 2, 3, 6).Merge();

        // -----------------------------------------------------
        // ENCABEZADOS
        // -----------------------------------------------------

        string[] headers =
        [
            Strings.ExcelHeaderCourse,
            Strings.ExcelHeaderStudent,
            Strings.ExcelHeaderDate,
            Strings.ExcelHeaderWrittenBy,
            Strings.ExcelHeaderCategory,
            Strings.ExcelHeaderDescription
        ];

        for (int column = 1; column <= headers.Length; column++)
        {
            worksheet.Cell(headerRow, column).Value = headers[column - 1];
        }

        IXLRange headerRange = worksheet.Range(headerRow, 1, headerRow, headers.Length);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // -----------------------------------------------------
        // ANOTACIONES
        // -----------------------------------------------------

        int row = firstDataRow;

        foreach (Student student in report.Students)
        {
            foreach (Annotation annotation in student.Annotations)
            {
                string teacher = _scheduleService.ResolveTeacher(report.Course, annotation.EnteredBy);

                worksheet.Cell(row, 1).Value = courseInfo.Code;
                worksheet.Cell(row, 2).Value = student.Name;
                worksheet.Cell(row, 3).Value = annotation.Date;
                worksheet.Cell(row, 3).Style.DateFormat.Format = Strings.DateTimeFormat;
                worksheet.Cell(row, 4).Value = teacher;
                worksheet.Cell(row, 5).Value = annotation.Category;
                worksheet.Cell(row, 6).Value = annotation.Description;
                row++;
                processedItems++;
                ReportExportProgress(progress, processedItems, totalItems, Strings.WritingAnnotations);
            }
        }

        // -----------------------------------------------------
        // TABLA
        // -----------------------------------------------------

        if (row > firstDataRow)
        {
            IXLRange dataRange = worksheet.Range(headerRow, 1, row - 1, headers.Length);
            dataRange.CreateTable();
        }

        // Mantener visibles metadatos + encabezados.
        worksheet.SheetView.FreezeRows(headerRow);

        // -----------------------------------------------------
        // DIMENSIONES
        // -----------------------------------------------------

        worksheet.Column(1).Width = 15;
        worksheet.Column(2).Width = 40;
        worksheet.Column(3).Width = 14;
        worksheet.Column(4).Width = 40;
        worksheet.Column(5).Width = 25;
        worksheet.Column(6).Width = 90;

        if (row > firstDataRow)
        {
            IXLRange contentRange = worksheet.Range(firstDataRow, 1, row - 1, headers.Length);
            contentRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            contentRange.Style.Alignment.WrapText = true;
        }
    }

    private void CreateSummaryWorksheet(XLWorkbook workbook, Report report, IProgress<OperationProgress>? progress, ref int processedItems, int totalItems)
    {
        IXLWorksheet worksheet = workbook.Worksheets.Add(Strings.SummaryWorksheetName);

        worksheet.Cell(1, 1).Value = Strings.ExcelHeaderCourse;
        worksheet.Cell(1, 2).Value = Strings.ExcelHeaderStudent;
        worksheet.Cell(1, 3).Value = Strings.ExcelHeaderAnnotations;

        IXLRange headerRange = worksheet.Range(1, 1, 1, 3);

        headerRange.Style.Font.Bold = true;

        int row = 2;

        foreach (Student student in report.Students)
        {
            worksheet.Cell(row, 1).Value = report.Course;
            worksheet.Cell(row, 2).Value = student.Name;
            worksheet.Cell(row, 3).Value = student.Annotations.Count;
            row++;
            processedItems++;

            ReportExportProgress(progress, processedItems, totalItems, Strings.GeneratingSummary);
        }

        if (row > 2)
        {
            worksheet.Range(1, 1, row - 1, 3).CreateTable();
        }

        worksheet.SheetView.FreezeRows(1);
        worksheet.Column(1).Width = 15;
        worksheet.Column(2).Width = 45;
        worksheet.Column(3).Width = 24;
    }

    private void ReportExportProgress(IProgress<OperationProgress>? progress, int processed, int total, string message)
    {
        if (total <= 0)
        {
            return;
        }

        int percentage = 5 + (int)Math.Round((double)processed / total * 85 );
        progress?.Report(new OperationProgress(percentage, message));
    }
}