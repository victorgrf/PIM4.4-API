using API;
using API.Data.Services;
using API.Data.ViewModels;
using API.DataBase;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add Json configurations.
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

// Add Services
builder.Services.AddTransient<AnalistaRHService>();
builder.Services.AddTransient<SecretarioService>();
builder.Services.AddTransient<ProfessorService>();
builder.Services.AddTransient<AlunoService>();
builder.Services.AddTransient<DisciplinaService>();
builder.Services.AddTransient<CursoService>();
builder.Services.AddTransient<TurmaService>();
builder.Services.AddTransient<DisciplinaMinistradaService>();

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
builder = Authorization.AddCustomSwaggerGen(builder); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();