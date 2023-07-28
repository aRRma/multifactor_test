using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace multifactor_test.Controllers
{
    public class PingDto
    {
        [Required]
        public string Id { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpPost]
        public Task<IActionResult> Ping(PingDto dto) => Task.FromResult<IActionResult>(Ok());
    }
}
