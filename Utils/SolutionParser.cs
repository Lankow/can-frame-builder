using CanFrameBuilder.Model;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

public class SolutionParser
{
    private const string WindowCaption = "Solution Parsing";

    public static bool ExtractProjects(ref Solution solution)
    {
        if (solution == null)
        {
            MessageBox.Show("Solution cannot be null.", WindowCaption,
                MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        var projectLinePattern = new Regex(
            @"^Project\(""\{.*?\}""\)\s=\s""(.*?)"",\s""(.*?)"",\s""\{.*?\}""",
            RegexOptions.Compiled);

        foreach (var line in File.ReadLines(solution.Path))
        {
            var match = projectLinePattern.Match(line.Trim());
            if (match.Success)
            {
                if(match.Groups.Count < 3)
                {
                    MessageBox.Show("Invalid Solution - Corrupted Project Data.", WindowCaption,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                var projectName = match.Groups[1].Value;
                var csprojPath = match.Groups[2].Value;

                solution.Projects.Add(new ProjectDetails(projectName, csprojPath));
            }
        }

        return true;
    }
}