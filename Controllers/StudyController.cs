using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using color_picker_server.Models;
using Microsoft.AspNetCore.Identity;


namespace color_picker_server.Controllers;

[ApiController]
[Route("[controller]")]
public class StudyController : ControllerBase
{

  private readonly Cloudinary _cloudinary;
  private readonly UserManager<User> _userManager;
  private readonly DBContext _context;

  public StudyController(
    DBContext context,
    UserManager<User> userManager,
    Cloudinary cloudinary
    )
  {
    _context = context;
    _userManager = userManager;
    _cloudinary = cloudinary;
  }


}
