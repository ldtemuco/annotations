namespace AnnotationsExtractor.Models;

public sealed class Report
{
    public required string FileName { get; init; }

    public required string Course { get; init; }

    public required IReadOnlyList<Student> Students { get; init; }
}