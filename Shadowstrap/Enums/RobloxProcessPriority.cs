using Shadowstrap.Models.Attributes;

namespace Shadowstrap.Enums
{
    public enum RobloxProcessPriority
    {
        [EnumName(StaticName = "Default")]
        Default,

        [EnumName(StaticName = "Above Normal")]
        AboveNormal,

        [EnumName(StaticName = "High")]
        High,

        [EnumName(StaticName = "Realtime")]
        Realtime
    }
}
