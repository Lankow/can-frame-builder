using CanFrameBuilder.Model;
using CanFrameBuilder.MVVM;
using CanFrameBuilder.Utils;
using CanFrameBuilder.View;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime;
using System.Windows;

namespace CanFrameBuilder.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<CANFrame> Frames { get; set; } = [];
        public Settings Settings { get; set; } = new Settings();

        public string ConfigDirectory { get; set; } = Directory.GetCurrentDirectory();

        private CANFrame? _selectedItem = null;

        public CANFrame? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddCommand => new(execute => AddFrame());
        public RelayCommand EditCommand => new(execute => EditFrame(), canExecute => _selectedItem != null);
        public RelayCommand DeleteCommand => new(execute => DeleteFrame(), canExecute => _selectedItem != null);
        public RelayCommand ClearCommand => new(execute => ClearFrames(), canExecute => Frames.Count > 0);
        public RelayCommand LoadCommand => new(execute => LoadConfig());
        public RelayCommand SettingsCommand => new(execute => OpenSettings());

        private void AddFrame()
        {
            var canFrameModal = new CANFrameModal(new CANFrame());
            canFrameModal.ShowDialog();

            if (!canFrameModal.Success) return;

            var frameToAdd = canFrameModal.Frame;
            Frames.Add(frameToAdd);
        }

        private void EditFrame()
        {
            var canFrameModal = new CANFrameModal(_selectedItem);
            canFrameModal.ShowDialog();

            if (!canFrameModal.Success) return;

            var frameToEdit= canFrameModal.Frame;
            int selectedItemIndex = Frames.IndexOf(_selectedItem);
            Frames[selectedItemIndex] = frameToEdit;
        }

        private void DeleteFrame()
        {
            var result = MessageBox.Show($"Are You sure You want to delete this item?",
                "Delete CAN Frame", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                Frames.Remove(_selectedItem);
        }

        private void ClearFrames()
        {
            var result = MessageBox.Show("Are You sure You want to clear CAN Frames list?",
                "Clear Messages", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Frames.Clear();
            }
        }

        private void LoadConfig()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "JSON Config Files | *.json",
                InitialDirectory = ConfigDirectory,
                Title = "Pick CAN Frames JSON config file"
            };

            var success = fileDialog.ShowDialog();

            if (success != true) return;

            ConfigDirectory = fileDialog.FileName;

            var configData = JSONHandler.LoadFromFile<ConfigData>(ConfigDirectory, "Config") ?? new ConfigData();

            if (configData is null) return;

            Frames.Clear();
            foreach (var frame in configData.Frames)
            {
                Frames.Add(frame);
            }

            Settings = configData.Settings;
        }

        private void OpenSettings()
        {
            var settingsModal = new SettingsModal(Settings);
            settingsModal.ShowDialog();
        }
    }
}
