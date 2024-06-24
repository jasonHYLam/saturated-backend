using Microsoft.AspNetCore.Identity;

namespace color_picker_server.Models
{
  public class User : IdentityUser
  {
    public ICollection<Study> Studies { get; set; } = new List<Study>();
  }
}