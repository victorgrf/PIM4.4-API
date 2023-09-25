using API.Data.Services;
using API.Data.ViewModels;
using API.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add Json configurations.
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Add Services
builder.Services.AddTransient<AnalistaRHService>();
builder.Services.AddTransient<SecretarioService>();

// Connect to MySQL DataBase.
var connectionStr = builder.Configuration.GetConnectionString("ConnectionMySQL");
builder.Services.AddDbContext<DBContext>(option => option.UseMySql(
    builder.Configuration.GetConnectionString("ConnectionMySQL"),
    ServerVersion.Parse(builder.Configuration.GetConnectionString("MySQLVersion"))
));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
