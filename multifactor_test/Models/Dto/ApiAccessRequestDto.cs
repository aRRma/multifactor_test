using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace multifactor_test.Models.Dto
{
    public class ApiAccessRequestDto
    {
        [Required, StringLength(100, MinimumLength = 1), JsonProperty(PropertyName = "resource", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)] 
        public string Resource { get; init; }

        [Required, Range(0, 1), JsonProperty(PropertyName = "action")]
        public ActionType Action { get; init; }
    }
}