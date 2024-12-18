using CanFrameBuilder.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace CanFrameBuilder
{
    internal class ConfigHandler
    {
        internal static void SaveConfig(string configOutputPath, List<CANFrame> canFrames)
        {
            try
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var jsonString = JsonSerializer.Serialize(canFrames, jsonSerializerOptions);
                File.WriteAllText(configOutputPath, jsonString);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Encountered error when saving Config file: {configOutputPath}." +
                    $" Error: {ex.Message}", "Config Save ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal static List<CANFrame> LoadConfig(string configInputPath)
        {
            try
            {
                var jsonText = File.ReadAllText(configInputPath);
                var deserializedConfig = JsonSerializer.Deserialize<List<CANFrame>>(jsonText);

                return deserializedConfig ?? [];
            }catch(Exception ex)
            {
                MessageBox.Show($"Encountered error when loading Config file: {configInputPath}." +
                    $" Error: {ex.Message}", "Config Load ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return [];
            }
        }
    }
}
