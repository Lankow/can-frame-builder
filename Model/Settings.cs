using System.Collections.ObjectModel;
using System.IO;

namespace CanFrameBuilder.Model
{
    public class Settings
    {
        public string? OutputDirectory { get; set; }
        public string? SolutionDirectory { get; set; }
        public bool AddNamespace { get; set; }
        public bool AddToProject { get; set; }
        public ObservableCollection<string> Imports { get; set; }

        public Settings()
        {
            OutputDirectory = Directory.GetCurrentDirectory();
            SolutionDirectory = "";
            AddNamespace = true;
            AddToProject = true;
            Imports = [];
        }
    }
}
