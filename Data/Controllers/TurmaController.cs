using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;
using Microsoft.AspNetCore.Authorization;
using API.Data.Identity;
using Microsoft.Extensions.Configuration;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TurmaController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly TurmaService service;

        public TurmaController(DBContext context, TurmaService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<List<Turma>> HttpGetAll(string? nome)
        {
            //AuthenticateService.ChecarCargo(new[] { "secretario" }, );

            var response = this.service.ServiceGetAll(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<Turma> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPost(Turma_Input turma)
        {
            var test_nome = this.dbContext.Turmas.Where(e => e.nome == turma.nome).FirstOrDefault();
            if (test_nome != null)
            {
                if (test_nome != null)
                {
                    var errorObj = new DuplicatedFieldError();
                    errorObj.AddField("nome");
                    return StatusCode(errorObj.GetStatusCode(), errorObj);
                }
            }

            var curso = this.dbContext.Cursos.FirstOrDefault(e => e.id == turma.idCurso);
            if (curso == null) return BadRequest("idCurso inválido");

            this.service.ServicePost(turma);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPut(int id, Turma_Input turma)
        {
            var test_nome = this.dbContext.Turmas.Where(e => e.nome == turma.nome).Where(e => e.id != id).FirstOrDefault();
            if (test_nome != null)
            {
                var errorObj = new DuplicatedFieldError();
                errorObj.AddField("nome");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.Turmas.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");

            var curso = this.dbContext.Cursos.FirstOrDefault(e => e.id == turma.idCurso);
            if (curso == null) return BadRequest("idCurso inválido");

            this.service.ServicePut(id, turma);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Turmas.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
