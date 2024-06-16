namespace color_picker_server.Models
{
  public class Study
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string OriginalLink { get; set; }
    public DateTime DateUploaded { get; set; }
  }
}