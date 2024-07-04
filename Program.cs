using color_picker_server.Models;
using Microsoft.EntityFrameworkCore;
using dotenv.net;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
// Cloudinary set up
// =================

Account account = new Account(
  Environment.GetEnvironmentVariable("ASPNETCORE_CLOUDINARY_NAME"),
  Environment.GetEnvironmentVariable("ASPNETCORE_CLOUDINARY_API_KEY"),
  Environment.GetEnvironmentVariable("ASPNETCORE_CLOUDINARY_API_SECRET")
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
      policy.WithOrigins([Environment.GetEnvironmentVariable("ASPNETCORE_FRONTEND_DOMAIN")])
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials();
    });
});

builder.Services.ConfigureApplicationCookie(options =>
{
  // options.Cookie.SameSite = SameSiteMode.None;
  options.Cookie.SameSite = SameSiteMode.Strict;
  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
// =================

// DB set up
// =================
var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_PSQL_CONNECTION_STRING");
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
// app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapIdentityApi<User>();
app.MapControllers();


app.Run();
