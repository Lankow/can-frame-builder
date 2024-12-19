using CanFrameBuilder.Model;
using CanFrameBuilder.View;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CanFrameBuilder
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<CANFrame> Entries { get; set; }

        private const string DefaultDirectory = "C:\\";
        private string _inputDirectory = DefaultDirectory;
        private string _outputDirectory = DefaultDirectory;

        public MainWindow()
        {
            DataContext = this;
            Entries = [];
            InitializeComponent();
        }

        private void BtnLoadConfig_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "JSON Config Files | *.json",
                InitialDirectory = _inputDirectory,
                Title = "Pick CAN Frames JSON config file"
            };

            var sucess = fileDialog.ShowDialog();

            if (sucess != true) return;

            var configFilePath = fileDialog.FileName;

            _inputDirectory = configFilePath;
            var canFrames = ConfigHandler.LoadConfig(_inputDirectory);

            if (canFrames is null || canFrames.Count <= 0) return;
            Entries.Clear();

            foreach(var canFrame in canFrames) Entries.Add(canFrame);
        }

        private void BtnSaveConfig_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Config Files | *.json",
                InitialDirectory = _outputDirectory,
                Title = "Save CAN Frames JSON config file"
            };

            var success = saveFileDialog.ShowDialog();
            if (success != true) return;

            var configFilePath = saveFileDialog.FileName;
            _outputDirectory = configFilePath;

            ConfigHandler.SaveConfig(configFilePath, [.. Entries]);
        }

        private void BtnPickOutputPath_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                InitialDirectory = _outputDirectory
            };

            var result = dialog.ShowDialog();

            if (result != true) return;

            _outputDirectory = dialog.FolderName;
        }

        private void BtnAddFrame_OnClick(object sender, RoutedEventArgs e)
        {
            var canFrameModal = new CANFrameModal();
            canFrameModal.ShowDialog();

            if(!canFrameModal.Success) return;

            var frameName = canFrameModal.FrameName;
            var frameId = canFrameModal.FrameId;
            var frameDlc = canFrameModal.FrameDlc;
            var frameChannel = canFrameModal.FrameChannel;
            var signals = canFrameModal.Signals;

            Entries.Add(new CANFrame(frameName, frameId, frameDlc, frameChannel, [.. signals]));
        }

        private void BtnEditFrame_OnClick(object sender, RoutedEventArgs e)
        {
            if (FrameEntries.SelectedItems.Count == 0) return;

            var selectedItem = FrameEntries.SelectedItems.Cast<CANFrame>().FirstOrDefault();
            var canFrameModal = new CANFrameModal(selectedItem);
            canFrameModal.ShowDialog();

            if (!canFrameModal.Success) return;

            var frameName = canFrameModal.FrameName;
            var frameId = canFrameModal.FrameId;
            var frameDlc = canFrameModal.FrameDlc;
            var frameChannel = canFrameModal.FrameChannel;
            var signals = canFrameModal.Signals;

            int selectedItemIndex = Entries.IndexOf(selectedItem);
            Entries[selectedItemIndex] = new CANFrame(frameName, frameId, frameDlc, frameChannel, [.. signals]);
        }

        private void BtnDeleteFrame_OnClick(object sender, RoutedEventArgs e)
        {
            if (FrameEntries.SelectedItems.Count == 0) return;

            var selectedItems = FrameEntries.SelectedItems.Cast<CANFrame>().ToList();
            var result = MessageBox.Show($"Are You sure You want to delete {selectedItems.Count} item(s)?",
                "Delete CAN Frames", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                foreach(var selectedItem in selectedItems)
                {
                    Entries.Remove(selectedItem);
                }
            }
        }

        private void BtnClearFrames_OnClick(object sender, RoutedEventArgs e)
        {
            if (Entries.Count > 0)
            {
                var result = MessageBox.Show("Are You sure You want to clear CAN Frames list?", 
                    "Clear Messages", MessageBoxButton.YesNo);
                
                if (result == MessageBoxResult.Yes)
                {
                    Entries.Clear();
                }
            }
        }

        private void BtnGenerate_OnClick(object sender, RoutedEventArgs e)
        {
            CANFrameGenerator.GenerateClasses([.. Entries], _outputDirectory);
        }
    }
}