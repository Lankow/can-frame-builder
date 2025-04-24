using CanFrameBuilder.Model;
using CanFrameBuilder.MVVM;

namespace CanFrameBuilder.ViewModel;

public class CANFrameModalViewModel(CANFrame canFrame) : ViewModelBase
{
    public RelayCommand AddCommand => new(execute => AddSignal(), canExecute => Frame.Signals.Count < MaxSignalsAmount);
    public RelayCommand DeleteCommand => new(execute => DeleteSignal(), canExecute => _selectedItem != null);
    public RelayCommand ClearCommand => new(execute => ClearSignal(), canExecute => Frame.Signals.Count > 0);

    private CANFrame _frame = canFrame;
    private Signal? _selectedItem = null;

    private const int MaxSignalsAmount = 20;

    public CANFrame Frame
    {
        get => _frame;
        set
        {
            _frame = value;
            OnPropertyChanged();
        }
    }

    public Signal? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }

    private void AddSignal()
    {
        Frame.Signals.Add(new Signal());
    }

    private void DeleteSignal()
    {
        if (SelectedItem != null)
        {
            Frame.Signals.Remove(SelectedItem);
        }
    }

    private void ClearSignal()
    {
        Frame.Signals.Clear();
    }
}

