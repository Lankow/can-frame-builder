using CanFrameBuilder.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CanFrameBuilder.View
{
    public partial class CANFrameModal : Window
    {
        public CANFrame Frame => (DataContext as CANFrameModalViewModel)?.Frame ?? new CANFrame();
        public bool Success { get; private set; }

        public CANFrameModal(CANFrame canFrame)
        {   
            InitializeComponent();
            DataContext = new CANFrameModalViewModel(canFrame);
        }

        private void SubmitFrameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Success = true;
            Close();
        }

        private void CancelFrameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VarValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[_a-zA-Z][_a-zA-Z0-9]*$");
            var result = !regex.IsMatch(e.Text);

            if (result)
            {
                MessageBox.Show("Frame name shall include only characters valid for a class name in C#.",
                "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            e.Handled = result;
        }
    }
}
