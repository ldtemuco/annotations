using AnnotationsExtractor.Models;
using AnnotationsExtractor.Services;
using AnnotationsExtractor.Resources;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.IO;

namespace AnnotationsExtractor;

public partial class MainWindow : Window
{
    private readonly WordExtractorService _wordExtractor = new();

    private readonly ScheduleService _scheduleService;

    private readonly ExcelExportService _excelExporter;

    private Report? _currentReport;

    private bool IsOutputEnabled => ShowOutputCheckBox.IsChecked == true;

    public MainWindow()
    {     

        InitializeComponent();
        ApplyLocalizedStrings();       
        _scheduleService = new ScheduleService();
        _excelExporter = new ExcelExportService(_scheduleService);
       
    }

    private void UpdateOutput(Report report)
    {
        if (!IsOutputEnabled)
        {
            return;
        }

        OutputTextBox.Text = BuildReportOutput(report);
    }

    private void ApplyLocalizedStrings()
    {
        Title = Strings.WindowTitle;
        OpenReportButton.Content = Strings.OpenReportButton;
        BatchProcessButton.Content = Strings.BatchProcessButton;
        ExportExcelButton.Content = Strings.ExportExcelButton;
        ShowOutputCheckBox.Content = Strings.ShowOutputCheckBox;
        ReadStatusTextBlock.Text = Strings.ReadWaiting;
        ExportStatusTextBlock.Text = Strings.ExportPending;
    }

    private void ShowOutputCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (IsOutputEnabled)
        {
            OutputTextBox.Visibility = Visibility.Visible;
            if (_currentReport is not null)
            {
                OutputTextBox.Text = BuildReportOutput(_currentReport);
            }
        }
        else
        {
            OutputTextBox.Clear();
            OutputTextBox.Visibility = Visibility.Collapsed;
        }
    }

    private async void OpenReportButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new()
        {
            Title = Strings.SelectReportDialogTitle,
            Filter = Strings.WordDocumentFilter,
            Multiselect = false
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        _currentReport = null;

        OpenReportButton.IsEnabled = false;
        ExportExcelButton.IsEnabled = false;
        ReadProgressBar.Value = 0;
        ReadStatusTextBlock.Text = Strings.ReadPreparing;
        ExportProgressBar.Value = 0;
        ExportStatusTextBlock.Text = Strings.ExportPending;

        try
        {
            Progress<OperationProgress> progress =
                new(
                    value =>
                    {
                        ReadProgressBar.Value = value.Percentage;
                        ReadStatusTextBlock.Text = $"{value.Message} " + $"{value.Percentage}%";
                    }
                );

            
            _currentReport = await _wordExtractor.ExtractAsync(dialog.FileName, progress);
            UpdateOutput(_currentReport);
            ExportExcelButton.IsEnabled = true;

        }
        catch (Exception ex)
        {
            ReadStatusTextBlock.Text = Strings.ReadError;
            MessageBox.Show($"""{Strings.ReportReadError} {ex.Message} """, Strings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            OpenReportButton.IsEnabled = true;
        }
    }

    private async void BatchProcessButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog fileDialog = new(){Title = Strings.SelectBatchReportsDialogTitle, Filter = Strings.WordDocumentFilter, Multiselect = true};

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        OpenFolderDialog folderDialog = new(){Title =Strings.SelectBatchOutputFolderTitle, Multiselect = false};

        if (folderDialog.ShowDialog() != true)
        {
            return;
        }

        string[] filePaths = fileDialog.FileNames;
        string outputDirectory = folderDialog.FolderName;
        int successfulFiles = 0;
        List<string> errors = [];
        _currentReport = null;
        SetProcessingState(true);

        try
        {
            for (int index = 0; index < filePaths.Length; index++)
            {
                string filePath = filePaths[index];
                string fileName = Path.GetFileName(filePath);
                int fileNumber = index + 1;
                ReadProgressBar.Value = 0;
                ExportProgressBar.Value = 0;

                try
                {
                    Progress<OperationProgress>
                        readProgress =
                            new(
                                value =>
                                {
                                    ReadProgressBar.Value =
                                        value.Percentage;

                                    ReadStatusTextBlock.Text =
                                        string.Format(
                                            Strings.BatchReadProgress,
                                            fileNumber,
                                            filePaths.Length,
                                            fileName,
                                            value.Message,
                                            value.Percentage
                                        );
                                }
                            );

                    Report report = await _wordExtractor.ExtractAsync(filePath, readProgress);
                    _currentReport = report;
                    UpdateOutput(report);

                    string excelFileName = BuildExcelFileName(report);

                    string outputPath = GetAvailableFilePath(outputDirectory, excelFileName);

                    Progress<OperationProgress>
                        exportProgress =
                            new(
                                value =>
                                {
                                    ExportProgressBar.Value =
                                        value.Percentage;

                                    ExportStatusTextBlock.Text =
                                        string.Format(
                                            Strings.BatchExportProgress,
                                            fileNumber,
                                            filePaths.Length,
                                            fileName,
                                            value.Message,
                                            value.Percentage
                                        );
                                }
                            );

                    await _excelExporter.ExportAsync(report, outputPath, exportProgress);

                    successfulFiles++;
                }
                catch (Exception ex)
                {
                    errors.Add(
                        $"""
                    {fileName}
                    {ex}
                    """
                    );
                }
            }

            ShowBatchResult(successfulFiles, errors);
        }
        finally
        {
            SetProcessingState(false);
        }
    }

    private void SetProcessingState(bool isProcessing)
    {
        OpenReportButton.IsEnabled = !isProcessing;
        BatchProcessButton.IsEnabled = !isProcessing;
        if (isProcessing)
        {
            ExportExcelButton.IsEnabled = false;
        }
        else
        {
            ExportExcelButton.IsEnabled = _currentReport is not null;
        }
    }

    private static string GetAvailableFilePath(string directory, string fileName)
    {
        string filePath = Path.Combine(directory, fileName);

        if (!File.Exists(filePath))
        {
            return filePath;
        }

        string name = Path.GetFileNameWithoutExtension(fileName);
        string extension = Path.GetExtension(fileName);

        int index = 2;

        while (true)
        {
            string candidate = Path.Combine(directory, $"{name}_{index}{extension}");

            if (!File.Exists(candidate))
            {
                return candidate;
            }

            index++;
        }
    }

    private static void ShowBatchResult(int successfulFiles, List<string> errors)
    {
        if (errors.Count == 0)
        {
            MessageBox.Show(
                string.Format(Strings.BatchCompleted,successfulFiles),
                Strings.BatchCompletedTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

            return;
        }

        string errorDetails = string.Join(Environment.NewLine + Environment.NewLine, errors);

        Clipboard.SetText(errorDetails);

        MessageBox.Show(
            string.Format(Strings.BatchCompletedWithErrors, successfulFiles, errors.Count),
            Strings.BatchCompletedTitle,
            MessageBoxButton.OK,
            MessageBoxImage.Warning
        );
    }

    private async void ExportExcelButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentReport is null)
        {
            return;
        }

        SaveFileDialog dialog = new()
        {
            Title = Strings.SaveExcelDialogTitle,
            Filter = Strings.ExcelFileFilter,
            DefaultExt = ".xlsx",
            AddExtension = true,
            FileName = BuildExcelFileName(_currentReport)
        };
               

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        OpenReportButton.IsEnabled = false;
        ExportExcelButton.IsEnabled = false;
        ExportProgressBar.Value = 0;
        ExportStatusTextBlock.Text = Strings.ExportPreparing;

        try
        {
            Progress<OperationProgress> progress =
                new(
                    value =>
                    {
                        ExportProgressBar.Value =
                            value.Percentage;

                        ExportStatusTextBlock.Text =
                            $"{value.Message} " +
                            $"{value.Percentage}%";
                    }
                );

            await _excelExporter.ExportAsync(_currentReport, dialog.FileName, progress);

            MessageBox.Show(Strings.ExportCompleted, Strings.ExportCompletedTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            ExportStatusTextBlock.Text = Strings.WriteError;

            MessageBox.Show($"""{Strings.ExcelExportError} {ex.Message}""", Strings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            OpenReportButton.IsEnabled = true;
            ExportExcelButton.IsEnabled = _currentReport is not null;
        }
    }

    private static string BuildReportOutput(Report report)
    {
        StringBuilder output = new();
        int annotationCount = report.Students.Sum( student => student.Annotations.Count);
        int studentsWithoutAnnotations = report.Students.Count(student => student.Annotations.Count == 0);
        output.AppendLine($"{Strings.LogLabelFile} {report.FileName}");
        output.AppendLine($"{Strings.LogLabelCourse} {report.Course}");
        output.AppendLine($"{Strings.LogLabelStudents} {report.Students.Count}");
        output.AppendLine($"{Strings.LogLabelAnnotations} {annotationCount}");
        output.AppendLine($"{Strings.LogLabelEmptyAnnotations} {studentsWithoutAnnotations}");
        output.AppendLine();

        foreach (Student student in report.Students)
        {
            output.AppendLine(new string('=', 70));
            output.AppendLine($"{Strings.LogLabelStudent} {student.Name}");
            output.AppendLine($"{Strings.LogLabelAnnotations} {student.Annotations.Count}");
            output.AppendLine();

            foreach (Annotation annotation in student.Annotations)
            {
                output.AppendLine(new string('-', 70));
                output.AppendLine($"{Strings.LogLabelDate} {annotation.Date.ToString(Strings.DateTimeFormat)}");
                output.AppendLine($"{Strings.LogLabelWrittenBy} {annotation.EnteredBy}");
                output.AppendLine($"{Strings.LogLabelCategory} {annotation.Category}");
                output.AppendLine($"{Strings.LogLabelDescription} {annotation.Description}");
                output.AppendLine();
            }
        }

        return output.ToString();
    }

    private static string BuildExcelFileName(Report report)
    {
        string course = FormatCourseForFileName(report.Course);
        string date = DateTime.Today.ToString(Strings.DateTimeFormat);
        return string.Format(Strings.ExcelFileNameFormat, course, date);
    }

    private static string FormatCourseForFileName(string course)
    {
        return course.Replace(Strings.ReplaceLevelText, "", StringComparison.OrdinalIgnoreCase).Replace(Strings.ReplaceGradeSymbol, "").Replace(Strings.ReplaceGradeSymbol, "").Replace(" ", "").Trim();
    }
}