namespace AnnotationsExtractor.Models;

public sealed class Annotation
{
    public required DateTime Date { get; init; }

    public required string EnteredBy { get; init; }

    public required string Category { get; init; }

    public required string Description { get; init; }
}