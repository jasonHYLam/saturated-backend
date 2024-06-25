namespace color_picker_server.Models.DTO;

public class NoteDTO
{
  public int Id { get; set; }
  public string Text { get; set; }
  public string OriginalHexColor { get; set; }
  public string GuessedHexColor { get; set; }
  public double XOrdinateAsFraction { get; set; }
  public double YOrdinateAsFraction { get; set; }
}