namespace color_picker_server.Models.Input;

public class CreateStudyInput
{
  public string Title { get; set; }
  public string OriginalLink { get; set; }
  public IFormFile ImageFile { get; set; }
}