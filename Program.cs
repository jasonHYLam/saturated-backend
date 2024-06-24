using color_picker_server.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<DBContext>(
  options => options.UseNpgsql(connectionString));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
