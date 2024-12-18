using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanFrameBuilder.Model;
public enum ByteOrder
{
    Intel,
    Motorola
}

public class CANFrame
{
    public string? Name { get; set; }
    public int Id { get; set; }

    public List<Signal> Signals { get; set; }

    public CANFrame()
    {
        Signals = [];
    }

    public CANFrame(string name, int id, List<Signal> signals)
    {
        Name = name;
        Id = id;
        Signals = signals;
    }
}