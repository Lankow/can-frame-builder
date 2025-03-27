using CanFrameBuilder.Model;
using CanFrameBuilder.ViewModel;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanFrameBuilder.View
{
    public partial class CANFrameModal : Window
    {
        public string FrameName => (DataContext as CANFrameModalViewModel)?.FrameName ?? "";
        public int FrameId => (DataContext as CANFrameModalViewModel)?.FrameId ?? 0;
        public int FrameDlc => (DataContext as CANFrameModalViewModel)?.FrameDlc ?? 0;
        public int FrameChannel => (DataContext as CANFrameModalViewModel)?.FrameChannel ?? 0;
        public ObservableCollection<Signal> Signals => (DataContext as CANFrameModalViewModel)?.Signals ?? new ObservableCollection<Signal>();

        public bool Success { get; private set; }

        public CANFrameModal(CANFrame? canFrame = null)
        {
            InitializeComponent();
            DataContext = new CANFrameModalViewModel(canFrame);
        }

        private void SubmitFrameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (ValidateFrame())
            {
                Success = true;
                Close();
            }
        }

        private void CancelFrameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void IntValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                e.Handled = !IsInteger(textBox.Text);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            
            e.Handled = !IsNumeric(e.Text);
        }

        private static bool IsNumeric(string input)
        {
            var regex = new Regex("^[0-9]+$");
            return regex.IsMatch(input);
        }
        private bool ValidateFrame()
        {
            if (!ValidateFrameName(FrameName) ||
                !ValidateRange(FrameId, 0, int.MaxValue, "Frame ID") ||
                !ValidateRange(FrameDlc, 1, short.MaxValue, "Frame DLC") ||
                !ValidateRange(FrameChannel, 0, short.MaxValue, "Frame Channel") ||
                !ValidateSignals(Signals))
            {
                return false;
            }

            return true;
        }

        private static bool ValidateFrameName(string name)
        {
            if (!IsVariableName(name))
            {
                ShowErrorWindow("Frame name shall include only characters valid for a class name in C#.");
                return false;
            }
            return true;
        }

        private static bool IsVariableName(string input)
        {
            var regex = new Regex(@"^[_a-zA-Z][_a-zA-Z0-9]*$");
            return regex.IsMatch(input);
        }

        private static bool IsInteger(string input)
        {
            try
            {
                int result = Convert.ToInt32(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                ShowErrorWindow($"Value shall be between {Int32.MinValue} and {Int32.MaxValue}.");
                return false;
            }
        }

        private static bool ValidateRange<T>(T value, T minValue, T maxValue, string fieldName) where T : IComparable<T>
        {
            if (!IsWithinRange(value, minValue, maxValue))
            {
                ShowErrorWindow($"{fieldName} value shall be between {minValue} and {maxValue}.");
                return false;
            }
            return true;
        }

        private static bool IsWithinRange<T>(T input, T minValue, T maxValue) where T : IComparable<T>
        {
            return input.CompareTo(minValue) >= 0 && input.CompareTo(maxValue) <= 0;
        }

        private static bool ValidateSignals(ObservableCollection<Signal> signals)
        {
            foreach (var signal in signals)
            {
                if (!ValidateSignal(signal))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool ValidateSignal(Signal signal)
        {
            if (signal.Name == null || !IsVariableName(signal.Name))
            {
                ShowErrorWindow("Signal Name shall include only characters valid for a class name in C#.");
                return false;
            }

            if (!ValidateRange(signal.LSB, 0, byte.MaxValue, "Signal LSB") ||
                !ValidateRange(signal.MSB, 0, byte.MaxValue, "Signal MSB") ||
                !ValidateRange(signal.BitCount, 0, byte.MaxValue, "Signal Bit Count") ||
                !ValidateRange(signal.DefaultValue, 0, byte.MaxValue, "Signal Default Value"))
            {
                return false;
            }

            return true;
        }

        private static void ShowErrorWindow(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
