using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;
using Microsoft.AspNetCore.Authorization;
using API.Data.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ConteudoController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly ConteudoService service;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ConteudoController(DBContext context, ConteudoService service, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = context;
            this.service = service;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<List<Conteudo>> HttpGetAll()
        {
            var response = this.service.ServiceGetAll();
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<Conteudo> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPost([FromForm] Conteudo_Input_Post conteudo)
        {
            var disciplinaMinistrada = this.dbContext.Disciplinas.FirstOrDefault(e => e.id == conteudo.idDisciplinaMinistrada);
            if (disciplinaMinistrada == null)
            {
                var errorObj = new InvalidIdReferenceError();
                errorObj.AddId("disciplina", conteudo.idDisciplinaMinistrada);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            string pasta;
            string caminho;
            string novoNome;
            string url;

            if (conteudo.documento.ContentType != "application/pdf")
            {
                var errorObj = new InvalidFormatError();
                errorObj.AddFile("documento", conteudo.documento.ContentType, new List<String> { "application/pdf" });
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            pasta = Path.Combine(this.webHostEnvironment.ContentRootPath, "DataBase\\Files\\Conteudos\\");
            novoNome = conteudo.documento.FileName.Replace(".pdf", DateTime.Now.ToString("(dddd, dd MMMM yyyy hh-mm-ss)"));

            caminho = Path.Combine(pasta, novoNome) + ".pdf";
            url = "/api/file/certificado/" + novoNome + ".pdf";

            using (var stream = new FileStream(caminho, FileMode.Create)) conteudo.documento.CopyTo(stream);

            this.service.ServicePost(conteudo, url);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPut(int id, [FromForm] Conteudo_Input_Put conteudo)
        {
            var table = this.dbContext.Conteudos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");

            var disciplinaMinistrada = this.dbContext.Disciplinas.FirstOrDefault(e => e.id == conteudo.idDisciplinaMinistrada);
            if (disciplinaMinistrada == null)
            {
                var errorObj = new InvalidIdReferenceError();
                errorObj.AddId("disciplina", conteudo.idDisciplinaMinistrada);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            string? url = null;
            if (conteudo.documento != null)
            {
                string pasta;
                string caminho;
                string novoNome;

                if (conteudo.documento.ContentType != "application/pdf")
                {
                    var errorObj = new InvalidFormatError();
                    errorObj.AddFile("documento", conteudo.documento.ContentType, new List<String> { "application/pdf" });
                    return StatusCode(errorObj.GetStatusCode(), errorObj);
                }

                pasta = Path.Combine(this.webHostEnvironment.ContentRootPath, "DataBase\\Files\\Conteudos\\");
                novoNome = conteudo.documento.FileName.Replace(".pdf", DateTime.Now.ToString("(dddd, dd MMMM yyyy hh-mm-ss)"));

                caminho = Path.Combine(pasta, novoNome) + ".pdf";
                url = "/api/file/conteudo/" + novoNome + ".pdf";

                using (var stream = new FileStream(caminho, FileMode.Create)) conteudo.documento.CopyTo(stream);
            }

            this.service.ServicePut(id, conteudo, url, this.webHostEnvironment);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Conteudos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
