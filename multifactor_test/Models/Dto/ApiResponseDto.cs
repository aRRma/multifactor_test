using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace multifactor_test.Models.Dto
{
    public class ApiResponseDto
    {
        [JsonProperty(PropertyName = "resource", Required = Required.Always)] public string Resource { get; init; }
        [JsonProperty(PropertyName = "decision", Required = Required.Always), JsonConverter(typeof(StringEnumConverter))] public DecisionType Decision { get; init; }
        [JsonProperty(PropertyName = "reason", Required = Required.Always)] public string Reason { get; init; }
    }
}