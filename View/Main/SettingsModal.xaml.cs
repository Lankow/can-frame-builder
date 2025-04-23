using CanFrameBuilder.Model;
using System.Windows;

namespace CanFrameBuilder.ViewModel
{
    public partial class SettingsModal : Window
    {
        public Settings Settings => (DataContext as SettingsModalViewModel)?.Settings ?? new Settings();
        public bool Success { get; private set; }

        public SettingsModal(Settings settings)
        {
            InitializeComponent();
            DataContext = new SettingsModalViewModel(settings);
        }

        private void SaveSettingsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Success = true;
            Close();
        }

        private void CloseSettingsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
