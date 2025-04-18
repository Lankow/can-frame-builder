using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace CanFrameBuilder.View.Controls
{
    public partial class FrameInfo : UserControl
    {
        public FrameInfo()
        {
            InitializeComponent();
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
