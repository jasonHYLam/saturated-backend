using color_picker_server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace color_picker_server.Controllers;

class UserController : ControllerBase
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

  // public async Task<ActionResult> GetIsUserGuest()
  // {

  // }

}