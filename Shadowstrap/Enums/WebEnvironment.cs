using System.ComponentModel;

namespace Shadowstrap.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WebEnvironment
    {
        [Description("prod")]
        Production,

        [Description("stage")]
        Staging,

        [Description("dev")]
        Dev,

        [Description("pizza")]
        DevPizza,

        [Description("matt")]
        DevMatt,

        [Description("local")]
        Local
    }
}
