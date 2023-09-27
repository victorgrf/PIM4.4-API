using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;
using API.Data.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using API.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Hosting;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly AuthenticateService authenticate;
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileController(DBContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = context;
            this.authenticate = new AuthenticateService(this.dbContext, configuration);
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("conteudo/{file}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<dynamic> Conteudo(string file)
        {
            string pasta = Path.Combine(this.webHostEnvironment.ContentRootPath, "DataBase\\Files\\Conteudos\\");
            string caminho = Path.Combine(pasta, file);
            try
            {
                var stream = new FileStream(caminho, FileMode.Open)
                return File(stream, "application/octet-stream", file);
            }
            catch (System.IO.FileNotFoundException)
            {
                return NotFound("Arquivo não encontrado");
            }
        }
    }
}
