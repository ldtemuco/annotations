using AnnotationsExtractor.Models;
using AnnotationsExtractor.Resources;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AnnotationsExtractor.Services;

public sealed class ScheduleService
{
    private static readonly CultureInfo SpanishCulture = CultureInfo.GetCultureInfo("es-CL");
    private readonly Dictionary<string, CourseSchedule> _courses;
    public ScheduleService()
    {
        _courses = LoadSchedule();
    }

    public CourseInfo GetCourseInfo(string reportCourse)
    {
        string courseCode = NormalizeCourseCode(reportCourse);

        if (!_courses.TryGetValue(courseCode, out CourseSchedule? schedule))
        {
            throw new InvalidDataException(string.Format(Strings.ScheduleCourseNotFound, courseCode.ToUpperInvariant()));
        }

        return new CourseInfo(courseCode.ToUpperInvariant(), FormatPersonName(schedule.HeadTeacher), FormatSector(schedule.Sector));
    }

    public string ResolveTeacher(string reportCourse, string enteredBy)
    {
        if (string.IsNullOrWhiteSpace(enteredBy))
        {
            return enteredBy;
        }

        string courseCode;

        try
        {
            courseCode = NormalizeCourseCode(reportCourse);
        }
        catch
        {
            return enteredBy;
        }

        if (!_courses.TryGetValue(courseCode, out CourseSchedule? schedule))
        {
            return enteredBy;
        }

        string normalizedEnteredBy = NormalizeLookupValue(enteredBy);

        if (schedule.SubjectTeachers.TryGetValue(normalizedEnteredBy, out string? teacher))
        {
            return FormatPersonName(teacher);
        }

        return enteredBy;
    }

    public static string NormalizeCourseCode(string course)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(course);
        string normalized = course.ToLower(SpanishCulture).Replace("media", string.Empty).Replace("°", string.Empty).Replace("º", string.Empty);
        normalized = Regex.Replace(normalized, @"\s+", string.Empty);

        if (!Regex.IsMatch(normalized, @"^[1-4][a-f]$", RegexOptions.CultureInvariant))
        {
            throw new InvalidDataException(string.Format(Strings.InvalidCourseFormat, course));
        }

        return normalized;
    }

    private static Dictionary<string, CourseSchedule> LoadSchedule()
    {
        string sql = LoadSqlScript();
        using SqliteConnection connection = new("Data Source=:memory:");
        connection.Open();
        using SqliteTransaction transaction = connection.BeginTransaction();

        foreach (string statement in SplitSqlStatements(sql))
        {
            using SqliteCommand command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = statement;
            command.ExecuteNonQuery();
        }

        transaction.Commit();

        Dictionary<string, string> heads = ReadHeads(connection);
        Dictionary<string, string> sectors = ReadSectors(connection);
        Dictionary<string, CourseSchedule> courses = new(StringComparer.OrdinalIgnoreCase);

        foreach ((string course, string headTeacher) in heads)
        {
            if (!sectors.TryGetValue(course, out string? sector))
            {
                continue;
            }

            Dictionary<string, string> subjectTeachers = ReadSubjectTeachers(connection, course);
            HashSet<string> teachers = subjectTeachers.Values.ToHashSet(StringComparer.OrdinalIgnoreCase);

            courses[course] =
                new CourseSchedule
                {
                    HeadTeacher = headTeacher,
                    Sector = sector,
                    SubjectTeachers = subjectTeachers,
                    Teachers = teachers
                };
        }

        return courses;
    }

    private static Dictionary<string, string> ReadHeads(SqliteConnection connection)
    {
        Dictionary<string, string> result = new(StringComparer.OrdinalIgnoreCase);
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText =
            """
            SELECT course, boss
            FROM heads;
            """;

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            string course = reader.GetString(0);
            string teacher = reader.GetString(1);
            result[course] = teacher;
        }

        return result;
    }

    private static Dictionary<string, string> ReadSectors(SqliteConnection connection)
    {
        Dictionary<string, string> result = new(StringComparer.OrdinalIgnoreCase);
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText =
            """
            SELECT course, sectorname
            FROM sectors;
            """;

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            string course = reader.GetString(0);
            string sector = reader.GetString(1);
            result[course] = sector;
        }

        return result;
    }

    private static Dictionary<string, string> ReadSubjectTeachers(SqliteConnection connection, string course)
    {
        if (!Regex.IsMatch(course, @"^[1-4][a-f]$", RegexOptions.CultureInvariant))
        {
            throw new InvalidDataException(string.Format(Strings.InvalidCourseFormat,course));
        }

        Dictionary<string, string> result = new(StringComparer.OrdinalIgnoreCase);
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText =
            $"""
            SELECT subject, teacher
            FROM "{course}";
            """;

        using SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            string subject = reader.GetString(0);
            string teacher = reader.GetString(1);
            result[NormalizeLookupValue(subject)] = teacher;
        }

        return result;
    }

    private static string LoadSqlScript()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        using Stream? stream = assembly.GetManifestResourceStream("schedule.sql");

        if (stream is null)
        {
            throw new InvalidDataException(Strings.ScheduleResourceNotFound);
        }

        using StreamReader reader = new(stream, Encoding.UTF8);

        return reader.ReadToEnd();
    }

    private static IEnumerable<string> SplitSqlStatements(string sql)
    {
        return sql
            .Split(
                ';',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries
            )
            .Where(
                statement =>
                    !string.IsNullOrWhiteSpace(
                        statement
                    )
            );
    }

    private static string NormalizeLookupValue(string value)
    {
        string normalized = RemoveDiacritics(value.ToLower(SpanishCulture));
        normalized = Regex.Replace(normalized, @"[^\p{L}\p{N}]+", " ");
        return Regex.Replace(normalized, @"\s+", " ").Trim();
    }

    private static HashSet<string> GetTokens(string value)
    {
        return value
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries
            )
            .ToHashSet(
                StringComparer.OrdinalIgnoreCase
            );
    }

    private static string RemoveDiacritics(string value)
    {
        string decomposed = value.Normalize(NormalizationForm.FormD);

        StringBuilder output = new();

        foreach (char character in decomposed)
        {
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(character);

            if (category != UnicodeCategory.NonSpacingMark)
            {
                output.Append(character);
            }
        }

        return output
            .ToString()
            .Normalize(
                NormalizationForm.FormC
            );
    }

    private static string FormatPersonName(string value)
    {
        return SpanishCulture
            .TextInfo
            .ToTitleCase(
                value
                    .Trim()
                    .ToLower(SpanishCulture)
            );
    }

    private static string FormatSector(string value)
    {
        string result =
            SpanishCulture
                .TextInfo
                .ToTitleCase(
                    value
                        .Trim()
                        .ToLower(SpanishCulture)
                );

        return result.Replace(
            " De:",
            " de:",
            StringComparison.Ordinal
        );
    }

    private sealed class CourseSchedule
    {
        public required string HeadTeacher
        {
            get;
            init;
        }

        public required string Sector
        {
            get;
            init;
        }

        public required
            Dictionary<string, string>
            SubjectTeachers
        {
            get;
            init;
        }

        public required HashSet<string> Teachers
        {
            get;
            init;
        }
    }
}