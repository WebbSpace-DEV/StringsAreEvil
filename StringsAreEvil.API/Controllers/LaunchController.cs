using Global.Utility.Common;
using Global.Utility.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Globalization;
using StringsAreEvil.API.DTOs;

namespace StringsAreEvil.API.Controllers;

//[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class LaunchController : ControllerBase
{
    private static Enums.PayloadTypes _payloadType;

    public LaunchController(IOptions<AppSettingsOption> options)
    {
        // This, of course, could be rendered by reading the string directly, but there is 
        // considerable value in validating that manipulation of enum processes correctly.
        TextInfo ti = new CultureInfo("en-US", false).TextInfo;
        try
        {
            _payloadType = (Enums.PayloadTypes)Enum.Parse(typeof(Enums.PayloadTypes), ti.ToTitleCase(options.Value.PayloadType));
        }
        catch (ArgumentException ex)
        {
            Debug.WriteLine(String.Format("\nAn ArgumentException was thrown on \"options.Value.PayloadType\":\n{0}\n", ex.Message));
            _payloadType = Enums.PayloadTypes.Unknown;
        }
        catch (NullReferenceException ex)
        {
            Debug.WriteLine(String.Format("\n\"AppSettings.PayloadType\" is missing from the appsetting.json file:\n{0}\n", ex.Message));
            _payloadType = Enums.PayloadTypes.Unknown;
        }
    }

    [HttpGet]
    public ActionResult<MessageDto> GetStatus()
    {
        return Ok(new MessageDto { Value = String.Format("Using \"{0}\" as the PayloadType", _payloadType.ToString()) });
    }
}
