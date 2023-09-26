using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;
using API.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<List<Disciplina>> HttpGetAll(string? nome)
        {
            var response = this.service.ServiceGetAll(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<Disciplina> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPost(Disciplina_Input disciplina)
        {
            var test_nome = this.dbContext.Disciplinas.Where(e => e.nome == disciplina.nome).FirstOrDefault();
            if (test_nome != null)
            {
                var errorObj = new DuplicatedFieldError();
                errorObj.AddField("nome");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePost(disciplina);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPut(int id, Disciplina_Input disciplina)
        {
            var test_nome = this.dbContext.Disciplinas.Where(e => e.nome == disciplina.nome).Where(e => e.id != id).FirstOrDefault();
            if (test_nome != null)
            {
                var errorObj = new DuplicatedFieldError();
                errorObj.AddField("nome");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.Disciplinas.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServicePut(id, disciplina);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Disciplinas.FirstOrDefault(e => e.id == id);
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");

            var disciplinaMinistrada = this.dbContext.DisciplinaMinistradas.Where(e => e.idDisciplina == id).ToList();
            if (disciplinaMinistrada.Count > 0)
            {
                var ids = new List<int>();
                foreach (var t in disciplinaMinistrada) ids.Add(t.id);

                var errorObj = new RelatedTableError();
                errorObj.AddTable("disciplinaMinistrada", ids);

                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
