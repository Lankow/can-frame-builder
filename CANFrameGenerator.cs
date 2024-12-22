using CanFrameBuilder.Model;
using System.IO;
using System.Text;
using System.Windows;

namespace CanFrameBuilder
{
    internal class CANFrameGenerator
    {
        private static readonly string[] CANoeImports = ["Vector.CANoe.Runtime", "Vector.CANoe.TFS"];

        internal static void GenerateClasses(List<CANFrame> canFrames, string outputDirectory)
        {
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
                    MessageBox.Show($"Encountered error during {canFrame.Name} Generation." +
                        $" Error: {ex.Message}", "Generation ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            MessageBox.Show($"Finished generation for {canFrames.Count} item(s) under path {outputDirectory}.",
                "Generation Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static string GenerateClassContent(CANFrame canFrame)
        {
            var sb = new StringBuilder();

            // Imports
            foreach(var importLine in CANoeImports)
            {
                var importLineStr = $"using {importLine};";
                sb.AppendLine(importLineStr);
            }

            // Namespace
            sb.Append("\nnamespace Adjust\n{\n");

            //Class Declaration
            sb.AppendLine($"\tpublic class {canFrame.Name} : CANFrame\n\t{{");

            // Constructor
            sb.AppendLine($"\t\tpublic {canFrame.Name}() : base({canFrame.Id}, {canFrame.Dlc})\n\t\t{{\n\t\t\tChannel = {canFrame.Channel};\n\t\t}}\n");

            // Signal Fields
            foreach (var signal in canFrame.Signals)
            {
                var byteOrderStr = signal.ByteOrder == ByteOrder.Motorola ? "Motorola" : "Intel";
                var signalStr = $"\t\t[Signal(LSB = {signal.LSB}, MSB = {signal.MSB}, BitCount = {signal.BitCount}, ByteOrder = ByteOrder.{byteOrderStr})] public byte {signal.Name} = {signal.DefaultValue};";
                sb.AppendLine(signalStr);
            }

            // Close Class
            sb.AppendLine("\t}\n}");
            var classContent = sb.ToString();

            return classContent;
        }
    }
}
