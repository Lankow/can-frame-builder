using CanFrameBuilder.Model;
using System.IO;
using System.Text;
using System.Windows;

namespace CanFrameBuilder
{
    internal class CANFrameGenerator
    {
        private static readonly string[] CANoeImports =
        {
            "Vector.CANoe.Runtime",
            "Vector.CANoe.TFS"
        };

        internal static void GenerateClasses(List<CANFrame> canFrames, string outputDirectory)
        {
            if (canFrames == null || canFrames.Count == 0)
            {
                MessageBox.Show("No CAN frames provided.", "Generation Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            foreach (var canFrame in canFrames)
            {
                try
                {
                    var filePath = Path.Combine(outputDirectory, $"{canFrame.Name}.cs");
                    var classContent = GenerateClassContent(canFrame);
                    File.WriteAllText(filePath, classContent);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating {canFrame.Name}: {ex.Message}",
                        "Generation ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MessageBox.Show($"Finished generation for {canFrames.Count} frame(s) in:\n{outputDirectory}",
                "Generation Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static string GenerateClassContent(CANFrame frame)
        {
            var sb = new StringBuilder();

            // Imports
            foreach (var import in CANoeImports)
                sb.AppendLine($"using {import};");

            sb.AppendLine("\nnamespace Adjust");
            sb.AppendLine("{");

            // Class Declaration
            sb.AppendLine($"\tpublic class {frame.Name} : CANFrame");
            sb.AppendLine("\t{");

            // Count Tabs - Try to determine how deep nested is currently class.

            // Constructor
            sb.AppendLine($"\t\tpublic {frame.Name}() : base({frame.Id}, {frame.Dlc})");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tChannel = {frame.Channel};");
            sb.AppendLine("\t\t}");

            sb.AppendLine();

            // Signals
            foreach (var signal in frame.Signals)
                sb.AppendLine(GenerateSignalDeclaration(signal));

            // Close Class & Namespace
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string GenerateSignalDeclaration(Signal signal)
        {
            var byteOrder = signal.ByteOrder == ByteOrder.Motorola ? "Motorola" : "Intel";
            return $"\t\t[Signal(LSB = {signal.LSB}, MSB = {signal.MSB}, BitCount = {signal.BitCount}, ByteOrder = ByteOrder.{byteOrder})] public byte {signal.Name} = {signal.DefaultValue};";
        }
    }
}