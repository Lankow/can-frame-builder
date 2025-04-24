using System.Collections.ObjectModel;

namespace CanFrameBuilder.Model;

public class ProjectDetails(string projectName, string csProjectPath)
{
    public string ProjectName { get; set; } = projectName;
    public string CsProjectPath { get; set; } = csProjectPath;

    public ProjectDetails Clone()
    {
        return new ProjectDetails(ProjectName, CsProjectPath);
    }
}

public class Solution
{
    public string Path { get; set; } = "";
    public ObservableCollection<ProjectDetails> Projects { get; set; } = new();

    public Solution Clone()
    {
        return new Solution
        {
            Path = this.Path,
            Projects = new ObservableCollection<ProjectDetails>(this.Projects.Select(p => p.Clone()))
        };
    }
}
