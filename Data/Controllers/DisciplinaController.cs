using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class DisciplinaController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly DisciplinaService service;

        public DisciplinaController(DBContext context, DisciplinaService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        public ActionResult<List<Disciplina>> HttpGetAll(string? nome)
        {
            var response = this.service.GetAllDisciplinas(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        public ActionResult<Disciplina> HttpGet(int id)
        {
            var response = this.service.GetDisciplina(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        public IActionResult HttpPost(Disciplina_Input disciplina)
        {
            var test_nome = this.dbContext.Disciplinas.Where(e => e.nome == disciplina.nome).FirstOrDefault();
            if (test_nome != null)
            {
                var errorObj = new DuplicatedFieldError();
                errorObj.AddField("nome");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.AddDisciplina(disciplina);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult HttpPut(int id, Disciplina_Input disciplina)
        {
            var test_nome = this.dbContext.Disciplinas.Where(e => e.nome == disciplina.nome).Where(e => e.id != id).FirstOrDefault();
            if (test_nome != null && disciplina.nome == test_nome.nome)
            {
                var errorObj = new DuplicatedFieldError();
                errorObj.AddField("nome");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.Disciplinas.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.UpdateDisciplina(id, disciplina);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Disciplinas.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.RemoveDisciplina(table);
            return Ok();
        }
    }
}
