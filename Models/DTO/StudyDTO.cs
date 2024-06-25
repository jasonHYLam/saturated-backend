namespace color_picker_server.Models.DTO;

public class StudyDTO
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string OriginalLink { get; set; }

  public string ImageLink { get; set; }
  public DateTime DateUploaded { get; set; }

}