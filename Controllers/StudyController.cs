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
    Console.WriteLine("checking user");
    Console.WriteLine(user);

    if (user == null)
    {
      return NotFound();
    }

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
      },
      PublicIdPrefix = "saturated"
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

    _context.Studies.Add(newStudy);
    await _context.SaveChangesAsync();

    var createdStudyDTO = new StudyDTO
    {
      Id = newStudy.Id,
      Title = newStudy.Title,
      OriginalLink = newStudy.OriginalLink,
      DateUploaded = newStudy.DateUploaded
    };

    return CreatedAtAction(nameof(GetStudy), new { id = newStudy.Id }, createdStudyDTO);
  }

  [HttpGet("allStudies")]
  public async Task<ActionResult<IEnumerable<StudyPreviewDTO>>> GetAllStudies()
  {
    var user = await _userManager.GetUserAsync(HttpContext.User);
    Console.WriteLine("checking user");
    Console.WriteLine(user);
    if (user == null)
    {
      return NotFound();
    }

    IQueryable<Study> studiesQuery =
    from study in _context.Studies
    where study.UserId == user.Id
    select study;

    var allStudies = await studiesQuery.ToListAsync();
    Console.WriteLine("checking allStudies");
    allStudies.ForEach(i => Console.WriteLine(i));

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

  public async Task<ActionResult<StudyDTO>> GetStudy(long id)
  {
    var study = await _context.Studies.FindAsync(id);

    if (study == null)
    {
      return NotFound();
    }
    else
    {
      var studyDTO = new StudyDTO
      {
        Id = study.Id,
        Title = study.Title,
        OriginalLink = study.OriginalLink,
        DateUploaded = study.DateUploaded,
      };
      return studyDTO;
    }
  }

}
