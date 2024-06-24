using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using color_picker_server.Models;
using Microsoft.AspNetCore.Identity;
using postgresTest.Model;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;

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

  [HttpPost]
  public async Task<ActionResult<StudyDTO>> CreateStudy([FromForm] CreateStudyInput input)
  {

    var user = await _userManager.GetUserAsync(HttpContext.User);

    var uploadParams = new ImageUploadParams()
    {
      File = new FileDescription(
        input.ImageFile.FileName,
        input.ImageFile.OpenReadStream()
        ),
      EagerTransforms = new List<Transformation>()
      {
        new EagerTransformation()
        .AspectRatio("1.0")
        .Width(300)
        .Chain()
        .FetchFormat("webp")
      }
    };

    var uploadResult = _cloudinary.Upload(uploadParams);

    Study newStudy = new Study
    {
      Title = input.Title,
      OriginalLink = input.OriginalLink,
      ImageLink = uploadResult.SecureUrl.ToString(),
      ThumbnailLink = uploadResult.Eager[0].SecureUrl.ToString(),
      UserId = user.Id,
      DateUploaded = new DateTime(),
    };

    var createdStudyDTO = new StudyDTO
    {
      Id = newStudy.Id,
      Title = newStudy.Title,
      OriginalLink = newStudy.OriginalLink,
      DateUploaded = newStudy.DateUploaded
    };

    return CreatedAtAction(nameof(GetStudy), new { id = newStudy.Id }, createdStudyDTO);
  }

  public async Task<ActionResult<IEnumerable<StudyPreviewDTO>>> GetAllStudies()
  {
    var user = await _userManager.GetUserAsync(HttpContext.User);

    IQueryable<Study> studiesQuery =
    from study in _context.Studies
    where study.UserId == user.Id
    select study;

    var allStudies = await studiesQuery.ToListAsync();

    var allStudyDTOs = allStudies.Select(
      s => new StudyPreviewDTO
      {
        Id = s.Id,
        Title = s.Title,
        DateUploaded = s.DateUploaded,
        ThumbnailLink = s.ThumbnailLink,
      }
    ).ToList();

    return allStudyDTOs;
  }
}
