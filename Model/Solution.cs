using System.Collections.ObjectModel;

namespace CanFrameBuilder.Model
{
    public class ProjectDetails
    {
        public string ProjectName { get; set; }
        public string CsProjPath { get; set; }

        public ProjectDetails(string projectName, string csprojPath)
        {
            ProjectName = projectName;
            CsProjPath = csprojPath;
        }

        public ProjectDetails Clone()
        {
            return new ProjectDetails(ProjectName, CsProjPath);
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
}
