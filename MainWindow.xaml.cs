using CanFrameBuilder.Model;
using CanFrameBuilder.ViewModel;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;


namespace CanFrameBuilder
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<CANFrame> Frames => (DataContext as MainWindowViewModel)?.Frames ?? [];
        public Settings Settings => (DataContext as MainWindowViewModel)?.Settings ?? new Settings();

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
            if (Directory.Exists(Settings.OutputDirectory) && !Settings.OutputDirectory.Equals(string.Empty))
            {
                CANFrameGenerator.GenerateClasses([.. Frames], Settings.OutputDirectory);
            }
            else
            {
                MessageBox.Show("Generation requires VALID Output path.", "INVALID OUTPUT PATH", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}