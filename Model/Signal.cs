namespace CanFrameBuilder.Model;

public enum ByteOrder
{
    Default = 0,
    Intel,
    Motorola
}

public class Signal
{
    public string? Name { get; set; } = "";
    public ushort LSB { get; set; } = 0;
    public ushort BitCount { get; set; } = 1;
    public uint DefaultValue { get; set; } = 0;
    public ByteOrder ByteOrder { get; set; } = ByteOrder.Default;
    public bool Generated { get; set; } = true;

    public Signal Clone()
    {
        return new Signal
        {
            Name = this.Name,
            LSB = this.LSB,
            BitCount = this.BitCount,
            DefaultValue = this.DefaultValue,
            ByteOrder = this.ByteOrder,
            Generated = this.Generated
        };
    }
}
