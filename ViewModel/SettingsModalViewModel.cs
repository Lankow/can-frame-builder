using CanFrameBuilder.Model;
using CanFrameBuilder.MVVM;
using Microsoft.Win32;
using System.IO;

namespace CanFrameBuilder.ViewModel
{
    public class SettingsModalViewModel(Settings settings) : ViewModelBase
    {
        public RelayCommand OutputCommand => new(execute => PickOutput());
        public RelayCommand SolutionCommand => new(execute => PickSolution());
        public RelayCommand LoadCommand => new(execute => LoadSettings());

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
            get => _settings.SolutionPath;
            set
            {
                _settings.SolutionPath = value;
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

            OutputDirectory = dialog.FolderName;
        }

        private void PickSolution()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "C# Solution File | *.sln",
                InitialDirectory = Settings.SolutionPath == "" ? Directory.GetCurrentDirectory()  : Settings.SolutionPath,
                Title = "Pick Solution File"
            };

            var result = dialog.ShowDialog();

            if (result != true) return;

            SolutionPath = dialog.FileName;
        }

        private void LoadSettings()
        {
            var loadSettingsDialog = new SaveFileDialog
            {
                Filter = "JSON Setting File | *.json",
                InitialDirectory = Directory.GetCurrentDirectory(),
                Title = "Load Setting JSON file"
            };

            var sucess = loadSettingsDialog.ShowDialog();

            if (sucess != true) return;
            var settingsDirectory = loadSettingsDialog.FileName;

            Settings = JSONHandler.LoadSettings(settingsDirectory);
        }
    }
}
