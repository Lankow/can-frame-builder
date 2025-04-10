using System.Collections.ObjectModel;
using System.IO;

namespace CanFrameBuilder.Model
{
    public class Settings
    {
        private const string CANOE_IMPORTS = "Vector.CANoe.Runtime Vector.CANoe.TFS";

        public string? OutputDirectory { get; set; }
        public string? SolutionPath { get; set; }
        public string? Imports { get; set; }

        public bool AddNamespace { get; set; }
        public bool AddToProject { get; set; }
        public bool AddImports { get; set; }

        public Settings()
        {
            OutputDirectory = "";
            SolutionPath = "";
            Imports = CANOE_IMPORTS;
            AddNamespace = true;
            AddToProject = true;
            AddImports = true;
        }
    }
}
