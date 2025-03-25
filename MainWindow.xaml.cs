using CanFrameBuilder.Model;
using CanFrameBuilder.View;
using CanFrameBuilder.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;


namespace CanFrameBuilder
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<CANFrame> Frames => (DataContext as MainWindowViewModel)?.Frames ?? [];
        public string OutputDirectory => (DataContext as MainWindowViewModel)?.OutputDirectory ?? Directory.GetCurrentDirectory();
        public string ConfigDirectory => (DataContext as MainWindowViewModel)?.ConfigDirectory ?? Directory.GetCurrentDirectory();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void BtnSaveConfig_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Config Files | *.json",
                InitialDirectory = Directory.GetCurrentDirectory(),
                Title = "Save CAN Frames JSON config file"
            };

            var success = saveFileDialog.ShowDialog();
            if (success != true) return;

            var configFilePath = saveFileDialog.FileName;

            ConfigHandler.SaveConfig(configFilePath, [.. Frames]);
        }

        private void BtnGenerate_OnClick(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(OutputDirectory) && !OutputDirectory.Equals(string.Empty))
            {
                CANFrameGenerator.GenerateClasses([.. Frames], OutputDirectory);
            }
        }
    }
}