using CanFrameBuilder.Model;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CanFrameBuilder.ViewModel
{
    public partial class SettingsModal : Window
    {
        public Settings Settings => (DataContext as SettingsModalViewModel)?.Settings ?? new Settings();

        public SettingsModal(Model.Settings settings)
        {
            InitializeComponent();
            DataContext = new SettingsModalViewModel(settings);
        }

        private void CloseSettingsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void LoadSettingsBtn_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void SaveSettingsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Setting File | *.json",
                InitialDirectory = Directory.GetCurrentDirectory(),
                Title = "Save Setting JSON file"
            };

            var success = saveFileDialog.ShowDialog();
            if (success != true) return;

            var settingsFilePath = saveFileDialog.FileName;
            JSONHandler.SaveSettings(settingsFilePath, Settings);
        }
    }
}
