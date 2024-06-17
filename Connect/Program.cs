
using Connect.Application.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogger();
builder.Services.AddControllers();
builder.Services.ConfigureServices(builder.Configuration); // Configure services first
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors(); // Configure CORS after authentication

var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseCustomMiddlewares(); // Use custom middlewares after Swagger

app.Run();
