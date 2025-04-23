namespace CanFrameBuilder.Model
{
    public enum ByteOrder
    {
        Default = 0,
        Intel,
        Motorola
    }

    public class Signal
    {
        public string? Name { get; set; }
        public ushort LSB { get; set; }
        public ushort BitCount { get; set; }
        public uint DefaultValue { get; set; }
        public ByteOrder ByteOrder { get; set; }
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

}
