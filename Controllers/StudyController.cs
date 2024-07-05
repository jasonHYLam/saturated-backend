using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using color_picker_server.Models;
using Microsoft.AspNetCore.Identity;
using color_picker_server.Models.DTO;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using color_picker_server.Models.Input;
using Microsoft.AspNetCore.Authorization;

namespace color_picker_server.Controllers;

[ApiController]
[Authorize]
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
        .Width(300)
        .Height(300)
        .Crop("fill")
        .Chain()
        .FetchFormat("webp")
      },
      PublicIdPrefix = "saturated"
    };

    var uploadResult = _cloudinary.Upload(uploadParams);

    var studyTitle = input.Title;
    if (input.Title == null)
    {
      studyTitle = "Untitled";
    }

    Study newStudy = new Study
    {
      Title = studyTitle,
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

    return CreatedAtAction(nameof(CreateStudy), new { id = newStudy.Id }, createdStudyDTO);
  }

  [HttpGet("allStudies")]
  public async Task<ActionResult<IEnumerable<StudyPreviewDTO>>> GetAllStudies()
  {
    var user = await _userManager.GetUserAsync(HttpContext.User);
    if (user == null)
    {
      return NotFound();
    }

    // IQueryable<Study> studiesQuery =
    // from study in _context.Studies
    // where study.UserId == user.Id
    // select study;

    // var allStudies = await studiesQuery.ToListAsync();
    var allStudies = await _context.Studies.ToListAsync();

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

  [HttpGet("{studyId}")]
  public async Task<ActionResult<StudyDTO>> GetStudy(int studyId)
  {
    var study = await _context.Studies.Include(s => s.Notes).FirstOrDefaultAsync(s => s.Id == studyId);
    var user = await _userManager.GetUserAsync(HttpContext.User);

    if (study == null)
    {
      return NotFound();
    }
    else if (study.UserId != user.Id)
    {
      return Forbid();
    }
    else
    {
      var studyDTO = new StudyDTO
      {
        Id = study.Id,
        Title = study.Title,
        OriginalLink = study.OriginalLink,
        ImageLink = study.ImageLink,
        DateUploaded = study.DateUploaded,
        Notes = study.Notes,
      };
      return studyDTO;
    }
  }

  [HttpDelete("{studyId}")]
  public async Task<ActionResult> DeleteStudy(int? studyId)
  {
    if (studyId == null)
    {
      return NotFound();
    }
    else
    {

      var studyToDelete = await _context.Studies.SingleOrDefaultAsync(s => s.Id == studyId);
      if (studyToDelete == null)
      {
        return NotFound();
      }
      _context.Studies.Remove(studyToDelete);
      await _context.SaveChangesAsync();
      return Ok();
    }
  }
}
