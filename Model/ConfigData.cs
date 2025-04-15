
using System.Collections.ObjectModel;

namespace CanFrameBuilder.Model;

public class ConfigData
{
    public Settings Settings { get; set; } = new Settings();
    public ObservableCollection<CANFrame> Frames { get; set; } = [];
}