using CanFrameBuilder.Model;
using CanFrameBuilder.MVVM;
using Microsoft.Win32;

namespace CanFrameBuilder.ViewModel
{
    public class SettingsModalViewModel(Settings settings) : ViewModelBase
    {
        public RelayCommand OutputCommand => new(execute => PickOutput());

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
    }
}
