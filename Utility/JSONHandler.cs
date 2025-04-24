using System.IO;
using System.Text.Json;
using System.Windows;

namespace CanFrameBuilder.Utility;

internal class JSONHandler
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    internal static void SaveToFile<T>(string filePath, T data, string dataType)
    {
        try
        {
            var jsonString = JsonSerializer.Serialize(data, _jsonOptions);
            File.WriteAllText(filePath, jsonString);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving {dataType} to file:\n{filePath}\n\nDetails: {ex.Message}",
                $"{dataType} Save ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    internal static T? LoadFromFile<T>(string filePath, string dataType)
    {
        try
        {
            var jsonText = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonText, _jsonOptions);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading {dataType} from file:\n{filePath}\n\nDetails: {ex.Message}",
                $"{dataType} Load ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            return default;
        }
    }
}
