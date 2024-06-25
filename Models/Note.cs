namespace color_picker_server.Models
{
  public class Note
  {
    public int Id { get; set; }
    public string Text { get; set; }

    // Colors are stored in hex format
    public string OriginalHexColor { get; set; }
    public string GuessedHexColor { get; set; }

    public int StudyId { get; set; }

    public double XOrdinateAsFraction { get; set; }
    public double YOrdinateAsFraction { get; set; }
  }
}