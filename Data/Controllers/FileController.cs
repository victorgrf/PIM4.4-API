using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;
using API.Data.Identity;
using API.Data.WritersPDF;
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
                var stream = new FileStream(caminho, FileMode.Open);
                return File(stream, "application/octet-stream", file);
            }
            catch (System.IO.FileNotFoundException)
            {
                return NotFound("Arquivo não encontrado");
            }
        }

        [HttpGet("boletim/{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public IActionResult GetBoletim(int id)
        {
            var aluno = this.dbContext.Alunos.Where(n => n.id == id).FirstOrDefault();
            if (aluno == null)
            {
                var errorObj = new InvalidIdReferenceError();
                errorObj.AddId("aluno", id);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var boletim = new Boletim(this.webHostEnvironment, this.dbContext);
            boletim.Gerar(id);

            try
            {
                if (boletim.GetCaminho() == null) throw new System.IO.FileNotFoundException();
                if (boletim.GetNome() == null) throw new System.IO.FileNotFoundException();
                var stream = new FileStream(boletim.GetCaminho(), FileMode.Open);
                return File(stream, "application/octet-stream", boletim.GetNome());
            }
            catch (System.IO.FileNotFoundException)
            {
                return NotFound("Arquivo não encontrado");
            }
        }

        [HttpGet("historico/{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public IActionResult GetHistorico(int id)
        {
            // AINDA PARA IMPLEMENTAR
            return Ok();
        }

        [HttpGet("declaracao/{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public IActionResult GetDeclaracao(int id)
        {
            // AINDA PARA IMPLEMENTAR
            return Ok();
        }

        [HttpGet("relatorio/{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public IActionResult GetRelatorio(int id)
        {
            // AINDA PARA IMPLEMENTAR
            return Ok();
        }
    }
}
