﻿using CanFrameBuilder.Model;
using CanFrameBuilder.MVVM;
using System.Collections.ObjectModel;

namespace CanFrameBuilder.ViewModel
{
    public class CANFrameModalViewModel : ViewModelBase
    {
        public ObservableCollection<Signal> Signals { get; set; } = [];

        public RelayCommand AddCommand => new(execute => AddSignal(), canExecute => Signals.Count() < MaxSignalsAmount);
        public RelayCommand DeleteCommand => new(execute => DeleteSignal(), canExecute => _selectedItem != null);
        public RelayCommand ClearCommand => new(execute => ClearSignal(), canExecute => Signals.Count > 0);

        private const int MaxSignalsAmount = 20;

        public CANFrameModalViewModel(CANFrame? canFrame)
        {
            if (canFrame == null) return;

            FrameName = canFrame.Name;
            FrameId = canFrame.Id;
            FrameDlc = canFrame.Dlc;
            FrameChannel = canFrame.Channel;

            foreach (var signal in canFrame.Signals)
            {
                Signals.Add(signal);
            }
        }

        public string? FrameName { get; set; } = "";
        public int FrameId { get; set; } = 0;
        public int FrameDlc { get; set; } = 0;
        public int FrameChannel { get; set; } = 0;

        private Signal? _selectedItem = null;

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
            Signals.Add(new Signal());
        }

        private void DeleteSignal()
        {
            if(SelectedItem != null)
            {
                Signals.Remove(SelectedItem);
            }
        }

        private void ClearSignal()
        {
            Signals.Clear();
        }
    }
}
