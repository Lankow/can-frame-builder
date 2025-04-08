using CanFrameBuilder.Model;
using CanFrameBuilder.MVVM;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;

namespace CanFrameBuilder.ViewModel
{
    public class SettingsModalViewModel(Settings settings) : ViewModelBase
    {
        public RelayCommand OutputCommand => new(execute => PickOutput());
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

        private void PickOutput()
        {
            var dialog = new OpenFolderDialog
            {
                InitialDirectory = Settings.OutputDirectory
            };

            var result = dialog.ShowDialog();

            if (result != true) return;

            Settings.OutputDirectory = dialog.FolderName;
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
