using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public short LSB { get; set; }
        public short MSB { get; set; }
        public short BitCount { get; set; }
        public byte DefaultValue { get; set; }
        public ByteOrder ByteOrder { get; set; }
    }
}
