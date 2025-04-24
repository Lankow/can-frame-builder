using CanFrameBuilder.Model;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace CanFrameBuilder.Utility;

public partial class SolutionParser
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

        var projectLinePattern = ProjectLinePattern();

        foreach (var line in File.ReadLines(solution.Path))
        {
            var match = projectLinePattern.Match(line.Trim());
            if (match.Success)
            {
                if (match.Groups.Count < 3)
                {
                    MessageBox.Show("Invalid Solution - Corrupted Project Data.", WindowCaption,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                var projectName = match.Groups[1].Value;
                var solutionDirectory = Path.GetDirectoryName(solution.Path);
                var csProjectPath = Path.Combine(solutionDirectory ?? Directory.GetCurrentDirectory(), match.Groups[2].Value);

                solution.Projects.Add(new ProjectDetails(projectName, csProjectPath));
            }
        }

        return true;
    }

    [GeneratedRegex(@"^Project\(""\{.*?\}""\)\s=\s""(.*?)"",\s""(.*?)"",\s""\{.*?\}""", RegexOptions.Compiled)]
    private static partial Regex ProjectLinePattern();
}
