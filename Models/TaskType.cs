using System.Text.Json.Serialization;

namespace KiwiTaskAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskType
    {
        offline,
        remote
    }
}
