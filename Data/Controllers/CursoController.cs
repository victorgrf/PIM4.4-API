using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;

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
        public ActionResult<List<Curso>> HttpGetAll(string? nome)
        {
            var response = this.service.ServiceGetAll(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        public ActionResult<Curso> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
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
        public IActionResult HttpPut(int id, Curso_Input curso)
        {
            var test_nome = this.dbContext.Cursos.Where(e => e.nome == curso.nome).Where(e => e.id != id).FirstOrDefault();
            if (test_nome != null && curso.nome == test_nome.nome)
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
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Cursos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
