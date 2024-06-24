using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace color_picker_server.Models
{
  public class DBContext : IdentityDbContext<User>
  {
    public DbSet<User> Users { get; set; }
    public DbSet<Study> Studies { get; set; }
    public DbSet<Note> Notes { get; set; }

    public DBContext(DbContextOptions<DBContext> options) : base(options) { }
  }


}