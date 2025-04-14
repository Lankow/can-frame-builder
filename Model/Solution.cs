using System.Collections.ObjectModel;

namespace CanFrameBuilder.Model
{
    public class ProjectDetails(string projectName, string csprojPath)
    {
        private string ProjectName = projectName;
        public string CsProjPath = csprojPath;
    }

    public class Solution
    {
        public string Path { get; set; } = "";
        public ObservableCollection<ProjectDetails> Projects { get; set; } = [];
    }
}
