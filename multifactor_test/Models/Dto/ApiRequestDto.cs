using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace multifactor_test.Models.Dto
{
    public class ApiRequestDto
    {
        [Required, StringLength(100, MinimumLength = 1), JsonProperty(PropertyName = "resource", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)] 
        public string Resource { get; init; }
    }
}