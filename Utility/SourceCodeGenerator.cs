using CanFrameBuilder.Model;
using System.IO;
using System.Xml.Linq;

namespace CanFrameBuilder.Utility
{
    public class SourceCodeGenerator
    {
        private const string NonGeneratedStartTag = "// NON-GENERATED : START";
        private const string NonGeneratedEndTag = "// NON-GENERATED : END";

        private readonly Settings _settings;

        public SourceCodeGenerator(Settings settings)
        {
            _settings = settings;
        }

        public void Generate(List<CANFrame> frames)
        {
            foreach (var frame in frames)
            {
                var className = frame.Name ?? throw new InvalidOperationException("Frame Name is required");
                var filePath = Path.Combine(_settings.OutputDirectory, $"{className}.cs");

                Directory.CreateDirectory(_settings.OutputDirectory);

                string preservedContent = string.Empty;
                if (File.Exists(filePath))
                {
                    preservedContent = ExtractNonGeneratedBlock(File.ReadAllText(filePath));
                }
                else
                {
                    preservedContent = $"{NonGeneratedStartTag}\n\n{NonGeneratedEndTag}";
                }

                var fileContent = GenerateClassContent(frame, preservedContent);
                File.WriteAllText(filePath, fileContent);

                if (_settings.AddToProject)
                {
                    foreach (var project in _settings.Solution.Projects)
                    {
                        AddFileToProject(project.CsProjPath, filePath);
                    }
                }
            }
        }

        private string GenerateClassContent(CANFrame frame, string customCodeBlock)
        {
            var cw = new CodeWriter();

            if (_settings.AddImports)
            {
                foreach (var import in _settings.Imports.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    cw.WriteLine($"using {import};");
                }
                cw.WriteLine();
            }

            string? ns = null;
            if (_settings.AddNamespace)
            {
                ns = CalculateNamespace(_settings.Solution.Path, _settings.OutputDirectory);
                cw.WriteLine($"namespace {ns}");
                cw.WriteLine("{");
                cw.Indent();
            }

            var className = frame.Name!;
            cw.WriteLine($"public class {className} : CANFrame");
            cw.WriteLine("{");
            cw.Indent();

            // Constructor
            cw.WriteLine($"public {className}() : base()");
            cw.WriteLine("{");
            cw.Indent();
            cw.WriteLine($"Id = {frame.Id};");
            cw.WriteLine($"Dlc = {frame.Dlc};");
            cw.WriteLine($"Channel = {frame.Channel};");
            cw.Unindent();
            cw.WriteLine("}");
            cw.WriteLine();

            // Signals
            foreach (var signal in frame.Signals)
            {
                var byteOrder = signal.ByteOrder == ByteOrder.Motorola ? "Motorola" : "Intel";
                var variableType = GetVariableType(signal.BitCount);

                cw.WriteLine($"[Signal(LSB = {signal.LSB}, BitCount = {signal.BitCount}, ByteOrder = ByteOrder.{byteOrder})]");
                cw.WriteLine($"public {variableType} {signal.Name} = {signal.DefaultValue};");
            }

            cw.WriteLine();
            cw.WriteLines(customCodeBlock.Split('\n').Select(line => line.TrimEnd()));

            cw.Unindent();
            cw.WriteLine("}");

            if (_settings.AddNamespace)
            {
                cw.Unindent();
                cw.WriteLine("}");
            }

            return cw.ToString();
        }

        private string ExtractNonGeneratedBlock(string fileContent)
        {
            var start = fileContent.IndexOf(NonGeneratedStartTag);
            var end = fileContent.IndexOf(NonGeneratedEndTag);

            if (start == -1 || end == -1 || end < start)
                return $"{NonGeneratedStartTag}\n\n{NonGeneratedEndTag}";

            return fileContent[start..(end + NonGeneratedEndTag.Length)];
        }

        private string CalculateNamespace(string solutionPath, string outputDirectory)
        {
            var solutionDir = Path.GetDirectoryName(solutionPath)!;
            var relativePath = Path.GetRelativePath(solutionDir, outputDirectory);

            var parts = relativePath
                .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .Select(part => part.Replace(" ", "_"));

            var rootNamespace = Path.GetFileNameWithoutExtension(solutionPath);
            return $"{rootNamespace}.{string.Join('.', parts)}";
        }

        private string GetVariableType(ushort bitCount)
        {
            return bitCount switch
            {
                > 32 => "ulong",
                > 16 => "uint",
                > 8 => "ushort",
                _ => "byte"
            };
        }

        private void AddFileToProject(string csprojPath, string filePath)
        {
            var relativePath = Path.GetRelativePath(Path.GetDirectoryName(csprojPath)!, filePath);
            var doc = XDocument.Load(csprojPath);
            var ns = doc.Root?.Name.Namespace ?? "";

            var itemGroup = doc.Descendants(ns + "ItemGroup")
                .FirstOrDefault(ig => ig.Elements(ns + "Compile").Any());

            if (itemGroup == null)
            {
                itemGroup = new XElement(ns + "ItemGroup");
                doc.Root?.Add(itemGroup);
            }

            bool alreadyIncluded = itemGroup.Elements(ns + "Compile")
                .Any(c => c.Attribute("Include")?.Value.Equals(relativePath, StringComparison.OrdinalIgnoreCase) == true);

            if (!alreadyIncluded)
            {
                itemGroup.Add(new XElement(ns + "Compile", new XAttribute("Include", relativePath)));
                doc.Save(csprojPath);
            }
        }
    }
}
