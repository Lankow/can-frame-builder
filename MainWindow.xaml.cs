using CanFrameBuilder.ViewModel;
using System.Windows;

namespace CanFrameBuilder;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
