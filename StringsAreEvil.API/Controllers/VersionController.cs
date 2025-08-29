using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StringsAreEvil.API.DTOs;

namespace StringsAreEvil.API.Controllers;

//[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class VersionController : ControllerBase
{
    [HttpGet]
    public ActionResult<MessageDto> GetVersion()
    {
        var ver = System.Reflection.Assembly.GetAssembly(typeof(VersionController)).GetName().Version;
        return Ok(new MessageDto { Value = String.Format("{0}", ver) });
    }
}
