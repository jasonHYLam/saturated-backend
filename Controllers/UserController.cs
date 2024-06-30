using color_picker_server.Models;
using dotenv.net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace color_picker_server.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class UserController : ControllerBase
{


  private readonly UserManager<User> _userManager;
  private readonly DBContext _context;
  public UserController(
    DBContext context,
    UserManager<User> userManager
    )
  {
    _context = context;
    _userManager = userManager;
  }


  [HttpGet("isGuest")]
  public async Task<ActionResult> GetIsUserGuest()
  {
    DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
    var user = await _userManager.GetUserAsync(HttpContext.User);

    if (user == null)
    {
      return NotFound();
    }

    var isGuest = user.Email == Environment.GetEnvironmentVariable("GUEST_USERNAME");
    Console.WriteLine(isGuest);

    return Ok(isGuest);
  }

}