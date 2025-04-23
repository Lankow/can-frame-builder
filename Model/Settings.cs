namespace CanFrameBuilder.Model
{
    public class Settings
    {
        private const string CANOE_IMPORTS = "Vector.CANoe.Runtime Vector.CANoe.TFS";

        public Solution Solution { get; set; } = new Solution();
        public string OutputDirectory { get; set; } = "";
        public string Imports { get; set; } = CANOE_IMPORTS;
        public bool AddNamespace { get; set; } = true;
        public bool AddToProject { get; set; } = true;
        public bool AddImports { get; set; } = true;

        public Settings Clone()
        {
            return new Settings
            {
                Solution = this.Solution.Clone(),
                OutputDirectory = this.OutputDirectory,
                Imports = this.Imports,
                AddNamespace = this.AddNamespace,
                AddToProject = this.AddToProject,
                AddImports = this.AddImports
            };
        }
    }
}
