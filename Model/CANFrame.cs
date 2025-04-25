using System.Collections.ObjectModel;

namespace CanFrameBuilder.Model;

public class CANFrame
{
    public string? Name { get; set; } = "";
    public uint Id { get; set; } = 0;
    public uint Dlc { get; set; } = 1;
    public uint Channel { get; set; } = 0;
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