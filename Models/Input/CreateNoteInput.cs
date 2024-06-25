namespace color_picker_server.Models.Input;

public class CreateNoteInput
{
  public string Text { get; set; }
  public string OriginalHexColor { get; set; }
  public string GuessedHexColor { get; set; }
  public double XOrdinateAsFraction { get; set; }
  public double YOrdinateAsFraction { get; set; }
}