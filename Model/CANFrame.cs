namespace CanFrameBuilder.Model;

public class CANFrame
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public int Dlc { get; set; }
    public int Channel { get; set; }   

    public List<Signal> Signals { get; set; }

    public CANFrame()
    {
        Signals = [];
    }

    public CANFrame(string name, int id, int dlc, int channel, List<Signal> signals)
    {
        Name = name;
        Id = id;
        Dlc = dlc;
        Channel = channel;
        Signals = signals;
    }
}