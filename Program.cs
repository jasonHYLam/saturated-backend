using color_picker_server.Models;
using Microsoft.EntityFrameworkCore;
using dotenv.net;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Cloudinary set up
// =================
DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));

Account account = new Account(
  Environment.GetEnvironmentVariable("CLOUDINARY_NAME"),
  Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
  Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
);
Cloudinary cloudinary = new Cloudinary(account);
cloudinary.Api.Secure = true;
builder.Services.AddSingleton(cloudinary);
// =================


// CORS set up
// =================
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

// Added this to solve cross site...
builder.Services.ConfigureApplicationCookie(options =>
{
  options.Cookie.SameSite = SameSiteMode.None;
  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
// =================

// DB set up
// =================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<DBContext>(
  options => options.UseNpgsql(connectionString)
  );
// =================

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddIdentityCore<User>()
.AddEntityFrameworkStores<DBContext>();
builder.Services.AddIdentityApiEndpoints<User>()
.AddEntityFrameworkStores<DBContext>();

// Swagger support
// =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
  config.DocumentName = "a";
  config.Title = "Lovely Rita";
  config.Version = "v1";
});
// =================

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
else
{
  app.UseOpenApi();
  app.UseSwaggerUi(config =>
  {
    config.DocumentTitle = "a";
    config.Path = "/swagger";
    config.DocumentPath = "/swagger/{documentName}/swagger.json";
    config.DocExpansion = "list";
  });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapIdentityApi<User>();
app.MapControllers();


app.Run();
