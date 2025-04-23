using CanFrameBuilder.ViewModel;
using System.Windows;

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
    }
}
