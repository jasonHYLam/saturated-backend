namespace postgresTest.Model;

public class CreateStudyInput
{
  public string Title { get; set; }
  public string OriginalLink { get; set; }
  public IFormFile ImageFile { get; set; }
}