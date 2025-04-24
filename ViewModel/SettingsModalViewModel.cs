using CanFrameBuilder.Model;
using CanFrameBuilder.MVVM;
using CanFrameBuilder.Utility;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace CanFrameBuilder.ViewModel;

public class SettingsModalViewModel(Settings settings) : ViewModelBase
{
    public RelayCommand OutputCommand => new(execute => PickOutput());
    public RelayCommand SolutionCommand => new(execute => PickSolution());

    private Settings _settings = settings;

    public Settings Settings
    {
        get => _settings;
        set
        {
            _settings = value;
            OnPropertyChanged();
        }
    }
    public string SolutionPath
    {
        get => _settings.Solution.Path;
        set
        {
            _settings.Solution.Path = value;
            OnPropertyChanged();
        }
    }

    public string OutputDirectory
    {
        get => _settings.OutputDirectory;
        set
        {
            _settings.OutputDirectory = value;
            OnPropertyChanged();
        }
    }

    private void PickOutput()
    {
        var dialog = new OpenFolderDialog
        {
            InitialDirectory = Settings.OutputDirectory == "" ? Directory.GetCurrentDirectory() : Settings.OutputDirectory
        };

        var result = dialog.ShowDialog();

        if (result != true) return;

        var path = dialog.FolderName;

        if (SolutionPath != "" && !PathHelper.IsSubPath(path, SolutionPath))
        {
            MessageBox.Show("Generation output has to be set in the same directory as Solution. Update Solution path.",
                "Invalid Solution Path", MessageBoxButton.OK, MessageBoxImage.Warning);
            SolutionPath = "";
        }

        OutputDirectory = path;
    }

    private void PickSolution()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "C# Solution File | *.sln",
            InitialDirectory = Settings.Solution.Path == "" ? Directory.GetCurrentDirectory() : Settings.Solution.Path,
            Title = "Pick Solution File"
        };

        var result = dialog.ShowDialog();

        if (result != true) return;

        var filePath = dialog.FileName;

        if (OutputDirectory != "" && !PathHelper.IsSubPath(OutputDirectory, filePath))
        {
            MessageBox.Show("Solution has to be set in the same directory as Generation output. Update Output path.",
                "Invalid Output Path", MessageBoxButton.OK, MessageBoxImage.Warning);
            OutputDirectory = "";
        }

        SolutionPath = dialog.FileName;

        var solution = Settings.Solution;
        if (SolutionParser.ExtractProjects(ref solution))
        {
            Settings.Solution = solution;
        }
    }
}
