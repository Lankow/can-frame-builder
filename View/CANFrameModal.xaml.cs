using CanFrameBuilder.Model;
using CanFrameBuilder.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CanFrameBuilder.View
{
    public partial class CANFrameModal : Window
    {
        public string FrameName => (DataContext as CANFrameModalViewModel)?.FrameName ?? "";
        public int FrameId => (DataContext as CANFrameModalViewModel)?.FrameId ??  0;
        public ObservableCollection<Signal> Signals => (DataContext as CANFrameModalViewModel)?.Signals ?? new ObservableCollection<Signal>();

        public bool Success { get; private set; }
        public CANFrameModal(CANFrame? canFrame = null)
        {
            InitializeComponent();
            var vm = new CANFrameModalViewModel(canFrame);
            DataContext = vm;
        }

        private void SubmitFrameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if(ValidateFrame())
            {
                Success = true;
                Close();
            }
        }

        private void CancelFrameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool ValidateFrame()
        {
            // Frame Validation Performed Here
            return true;
        }
    }
}
