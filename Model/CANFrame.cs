using System.Collections.ObjectModel;

namespace CanFrameBuilder.Model;

public class CANFrame
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public int Dlc { get; set; }
    public int Channel { get; set; }
    public bool Generated { get; set; } = true;

    public ObservableCollection<Signal> Signals { get; set; } = [];
}