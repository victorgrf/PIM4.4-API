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
    public class CursoController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly CursoService service;

        public CursoController(DBContext context, CursoService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<List<Curso>> HttpGetAll(string? nome)
        {
            //AuthenticateService.ChecarCargo(new[] { "secretario" }, );

            var response = this.service.ServiceGetAll(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<Curso> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPost(Curso_Input curso)
        {
            var test_nome = this.dbContext.Cursos.Where(e => e.nome == curso.nome).FirstOrDefault();
            if (test_nome != null)
            {
                if (test_nome != null)
                {
                    var errorObj = new DuplicatedFieldError();
                    errorObj.AddField("nome");
                    return StatusCode(errorObj.GetStatusCode(), errorObj);
                }
            }

            this.service.ServicePost(curso);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPut(int id, Curso_Input curso)
        {
            var test_nome = this.dbContext.Cursos.Where(e => e.nome == curso.nome).Where(e => e.id != id).FirstOrDefault();
            if (test_nome != null)
            {
                var errorObj = new DuplicatedFieldError();
                errorObj.AddField("nome");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.Cursos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServicePut(id, curso);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Cursos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");

            var turma = this.dbContext.Turmas.Where(e => e.idCurso == table.id).ToList();
            if (turma.Count > 0)
            {
                var ids = new List<int>();
                foreach (var t in turma) ids.Add(t.id);

                var errorObj = new RelatedTableError();
                errorObj.AddTable("turma", ids);

                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
