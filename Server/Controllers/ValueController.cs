using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtLogin.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ValueController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "aaa", "bbb" };
    } 
}