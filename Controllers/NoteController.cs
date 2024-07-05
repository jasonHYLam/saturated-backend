using CloudinaryDotNet;
using color_picker_server.Models;
using color_picker_server.Models.DTO;
using color_picker_server.Models.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace color_picker_server.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
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

  [HttpPost("{studyId}")]
  public async Task<ActionResult<NoteDTO>> CreateNote(int studyId, [FromBody] CreateNoteInput input)
  {
    if (studyId == null)
    {
      return NotFound();
    }
    Note newNote = new Note
    {
      Text = input.Text,
      OriginalHexColor = input.OriginalHexColor,
      GuessedHexColor = input.GuessedHexColor,
      XOrdinateAsFraction = input.XOrdinateAsFraction,
      YOrdinateAsFraction = input.YOrdinateAsFraction,
      StudyId = studyId
    };

    _context.Notes.Add(newNote);
    await _context.SaveChangesAsync();

    var createdNoteDTO = new NoteDTO
    {
      Id = newNote.Id,
      Text = newNote.Text,
      OriginalHexColor = newNote.OriginalHexColor,
      GuessedHexColor = newNote.GuessedHexColor,
      XOrdinateAsFraction = newNote.XOrdinateAsFraction,
      YOrdinateAsFraction = newNote.YOrdinateAsFraction,
    };

    return CreatedAtAction(nameof(CreateNote), new { id = newNote.Id }, createdNoteDTO);
  }

  [HttpPut("{noteId}")]
  public async Task<ActionResult<NoteDTO>> UpdateNote(int? noteId, [FromBody] UpdateNoteInput input)
  {
    if (noteId == null)
    {
      return NotFound();
    }
    var noteToUpdate = await _context.Notes.SingleOrDefaultAsync(n => n.Id == noteId);
    if (noteToUpdate == null)
    {
      return NotFound();
    }
    noteToUpdate.Text = input.Text;
    await _context.SaveChangesAsync();

    var updatedNoteDTO = new NoteDTO
    {
      Id = noteToUpdate.Id,
      Text = noteToUpdate.Text,
      OriginalHexColor = noteToUpdate.OriginalHexColor,
      GuessedHexColor = noteToUpdate.GuessedHexColor,
      XOrdinateAsFraction = noteToUpdate.XOrdinateAsFraction,
      YOrdinateAsFraction = noteToUpdate.YOrdinateAsFraction,
    };

    return CreatedAtAction(nameof(UpdateNote), new { id = noteToUpdate.Id }, updatedNoteDTO);
  }

  [HttpDelete("{noteId}")]
  public async Task<ActionResult> DeleteNote(int? noteId)
  {
    if (noteId == null)
    {
      return NotFound();
    }

    var noteToDelete = await _context.Notes.SingleOrDefaultAsync(n => n.Id == noteId);
    if (noteToDelete == null)
    {
      return NotFound();
    }
    _context.Notes.Remove(noteToDelete);
    await _context.SaveChangesAsync();

    return Ok();

  }
}