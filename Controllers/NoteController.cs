using CloudinaryDotNet;
using color_picker_server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace color_picker_server.Controllers;

public class NoteController : ControllerBase
{

  private readonly Cloudinary _cloudinary;
  private readonly UserManager<User> _userManager;
  private readonly DBContext _context;

  public NoteController(
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