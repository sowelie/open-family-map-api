using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenFamilyMapAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetStatus()
    {
        return Ok(new { 
            status = "OK", 
            timestamp = DateTime.UtcNow,
            openFamilyMapApiVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown"
        });
    }
}