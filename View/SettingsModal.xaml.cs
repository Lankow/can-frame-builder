using System.Windows;

namespace CanFrameBuilder.ViewModel
{
    public partial class SettingsModal : Window
    {
        public SettingsModal(Model.Settings settings)
        {
            InitializeComponent();
            DataContext = new SettingsModalViewModel(settings);
        }

        private void CloseSettingsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
