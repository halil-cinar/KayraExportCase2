using KayraExportCase2.API.Middlewares;
using KayraExportCase2.Application.Abstract;
using KayraExportCase2.Application.Caching;
using KayraExportCase2.Application.Services;
using KayraExportCase2.DataAccess.Abstract;
using KayraExportCase2.DataAccess.Context;
using KayraExportCase2.Domain;
using Microsoft.Extensions.Logging.Console;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
    options.FormatterName = ConsoleFormatterNames.Simple;
});

builder.Services.Configure<SimpleConsoleFormatterOptions>(options =>
{
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    //options.IncludeScopes = true;
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfGenericRepositoryBase<>));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddRedisCacheService(builder.Configuration);
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddHttpContextAccessor();
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);
AppSettings.Initialize(appSettings);
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
