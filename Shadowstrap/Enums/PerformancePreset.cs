using Shadowstrap.Models.Attributes;

namespace Shadowstrap.Enums
{
    public enum PerformancePreset
    {
        [EnumName(StaticName = "Default")]
        Default,

        [EnumName(StaticName = "Balanced")]
        Balanced,

        [EnumName(StaticName = "Max Performance")]
        MaxPerformance
    }
}
