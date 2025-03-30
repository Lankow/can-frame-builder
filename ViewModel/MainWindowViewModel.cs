using CanFrameBuilder.Model;
using CanFrameBuilder.MVVM;
using CanFrameBuilder.View;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace CanFrameBuilder.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<CANFrame> Frames { get; set; } = [];
        public Settings Settings { get; set; } = new Settings();

        public string ConfigDirectory { get; set; } = Directory.GetCurrentDirectory();
        public string OutputDirectory { get; set; } = Directory.GetCurrentDirectory();

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
        public RelayCommand OutputCommand => new(execute => PickOutput());

        private void AddFrame()
        {
            var canFrameModal = new CANFrameModal();
            canFrameModal.ShowDialog();

            if (!canFrameModal.Success) return;

            var frameToAdd = ExtractModalFrame(canFrameModal);
            Frames.Add(frameToAdd);
        }

        private void EditFrame()
        {
            var canFrameModal = new CANFrameModal(_selectedItem);
            canFrameModal.ShowDialog();

            if (!canFrameModal.Success) return;

            var frameToEdit= ExtractModalFrame(canFrameModal);
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

            var sucess = fileDialog.ShowDialog();

            if (sucess != true) return;

            ConfigDirectory = fileDialog.FileName;
            var canFrames = ConfigHandler.LoadConfig(ConfigDirectory);

            if (canFrames is null || canFrames.Count <= 0) return;
            Frames.Clear();

            foreach (var canFrame in canFrames) Frames.Add(canFrame);
        }

        private void PickOutput()
        {
            var dialog = new OpenFolderDialog
            {
                InitialDirectory = OutputDirectory
            };

            var result = dialog.ShowDialog();

            if (result != true) return;

            OutputDirectory = dialog.FolderName;
        }

        private CANFrame ExtractModalFrame(CANFrameModal canFrameModal)
        {
            var frameName = canFrameModal.FrameName;
            var frameId = canFrameModal.FrameId;
            var frameDlc = canFrameModal.FrameDlc;
            var frameChannel = canFrameModal.FrameChannel;
            var signals = canFrameModal.Signals;

            return new CANFrame(frameName, frameId, frameDlc, frameChannel, [.. signals]);
        }
    }
}
