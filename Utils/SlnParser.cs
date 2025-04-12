using System.IO;
using System.Text.RegularExpressions;

public class SlnParser
{
    public static List<(string ProjectName, string CsprojPath)> ExtractProjects(string slnFilePath)
    {
        var projects = new List<(string, string)>();
        var projectLinePattern = new Regex(
            @"^Project\(""\{.*?\}""\)\s=\s""(.*?)"",\s""(.*?)"",\s""\{.*?\}""",
            RegexOptions.Compiled);

        foreach (var line in File.ReadLines(slnFilePath))
        {
            var match = projectLinePattern.Match(line.Trim());
            if (match.Success)
            {
                var projectName = match.Groups[1].Value;
                var csprojPath = match.Groups[2].Value;
                projects.Add((projectName, csprojPath));
            }
        }

        return projects;
    }
}