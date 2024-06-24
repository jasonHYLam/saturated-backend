using color_picker_server.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
  options.AddPolicy(
    name: MyAllowSpecificOrigins,
    policy =>
    {
      policy.WithOrigins(["http://localhost:5173"])
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<DBContext>(
  options => options.UseNpgsql(connectionString)
  );

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>()
.AddEntityFrameworkStores<DBContext>();


var app = builder.Build();
app.MapIdentityApi<User>();

app.MapGet("/", () => "Hello World!");

app.Run();
