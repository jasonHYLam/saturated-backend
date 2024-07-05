using color_picker_server.Models;
using dotenv.net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace color_picker_server.Controllers;

[ApiController]
// [Authorize]
[Route("[controller]")]
public class UserController : ControllerBase
{

  private readonly SignInManager<User> _signInManager;
  private readonly UserManager<User> _userManager;
  public UserController(
    UserManager<User> userManager,
    SignInManager<User> signInManager
    )
  {
    _userManager = userManager;
    _signInManager = signInManager;
  }


  [HttpGet("isGuest")]
  // public async Task<ActionResult<bool>> GetIsUserGuest()
  // public async Task<ActionResult<string>> GetIsUserGuest()
  public ActionResult GetIsUserGuest()
  {
    DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
    // var user = await _userManager.GetUserAsync(HttpContext.User);

    // if (user == null)
    // {
    //   return NotFound();
    // }

    // var isGuest = user.Email == Environment.GetEnvironmentVariable("GUEST_USERNAME");
    var isGuest = Environment.GetEnvironmentVariable("GUEST_USERNAME");
    Console.WriteLine("CHECKING");
    Console.WriteLine(isGuest);

    // return Content("Haheheh");
    return Ok();
  }

  [HttpPost("logout")]
  public async Task<ActionResult> Logout()
  {
    await _signInManager.SignOutAsync();
    return Ok();
  }
}