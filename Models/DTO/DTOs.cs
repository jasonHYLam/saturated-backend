namespace postgresTest.Model;

public class StudyDTO
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string OriginalLink { get; set; }

  public string ImageLink { get; set; }
  public DateTime DateUploaded { get; set; }

}

public class StudyPreviewDTO
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string ThumbnailLink { get; set; }
  public DateTime DateUploaded { get; set; }
}

public class NoteDTO
{
  public string Text { get; set; }
  public string OriginalHexColor { get; set; }
  public string GuessedHexColor { get; set; }
  public double xOrdinateAsFraction { get; set; }
  public double yOrdinateAsFraction { get; set; }
}