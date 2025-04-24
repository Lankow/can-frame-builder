using CanFrameBuilder.Model;
using System.Text.RegularExpressions;
using System.Windows;

namespace CanFrameBuilder.ViewModel;

public partial class SettingsModal : Window
{
    public Settings Settings => (DataContext as SettingsModalViewModel)?.Settings ?? new Settings();
    public bool Success { get; private set; }

    public SettingsModal(Settings settings)
    {
        InitializeComponent();
        DataContext = new SettingsModalViewModel(settings);
    }

    private void SaveSettingsBtn_OnClick(object sender, RoutedEventArgs e)
    {
        string? imports = Settings?.Imports;

        if (string.IsNullOrWhiteSpace(imports) || !IsValidImports(imports))
        {
            MessageBox.Show("Only space-separated Namespace.ClassName entries allowed (e.g., Vector.CANoe.Runtime).",
                "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Success = true;
        Close();
    }


    private void CloseSettingsBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private static bool IsValidImports(string input)
    {
        var singleImportPattern = ImportsRegex();
        var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.All(p => singleImportPattern.IsMatch(p));
    }

    [GeneratedRegex(@"^[A-Za-z_][A-Za-z0-9_]*(\.[A-Za-z_][A-Za-z0-9_]*)+$")]
    private static partial Regex ImportsRegex();
}
