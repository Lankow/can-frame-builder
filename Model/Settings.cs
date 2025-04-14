namespace CanFrameBuilder.Model
{
    public class Settings
    {
        private const string CANOE_IMPORTS = "Vector.CANoe.Runtime Vector.CANoe.TFS";

        public Solution Solution { get; set; }

        public string OutputDirectory { get; set; }
        public string Imports { get; set; }

        public bool AddNamespace { get; set; }
        public bool AddToProject { get; set; }
        public bool AddImports { get; set; }

        public Settings()
        {
            Solution = new Solution();
            OutputDirectory = "";
            Imports = CANOE_IMPORTS;
            AddNamespace = true;
            AddToProject = true;
            AddImports = true;
        }
    }
}
