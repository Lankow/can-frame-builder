using CanFrameBuilder.Model;
using System.Collections.ObjectModel;

public class CANFrame
{
    public string? Name { get; set; }
    public uint Id { get; set; }
    public uint Dlc { get; set; }
    public uint Channel { get; set; }
    public bool Generated { get; set; } = true;

    public ObservableCollection<Signal> Signals { get; set; } = [];

    public CANFrame Clone()
    {
        return new CANFrame
        {
            Name = this.Name,
            Id = this.Id,
            Dlc = this.Dlc,
            Channel = this.Channel,
            Generated = this.Generated,
            Signals = new ObservableCollection<Signal>(this.Signals.Select(s => s.Clone()))
        };
    }
}