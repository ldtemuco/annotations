namespace AnnotationsExtractor.Models;

public sealed class Student
{
    public required string Name { get; init; }

    public required IReadOnlyList<Annotation> Annotations { get; init; }
}