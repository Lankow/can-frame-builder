using CanFrameBuilder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CanFrameBuilder
{
    internal class CANFrameGenerator
    {
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
                }
            }
        }

        private static string GenerateClassContent(CANFrame canFrame)
        {
            var classContent = $"\tpublic class {canFrame.Name} : CANFrame";
            
            return classContent;
        }
    }
}
