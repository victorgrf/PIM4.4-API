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
    public class DisciplinaMinistradaController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly DisciplinaMinistradaService service;

        public DisciplinaMinistradaController(DBContext context, DisciplinaMinistradaService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<List<DisciplinaMinistrada>> HttpGetAll()
        {
            var response = this.service.ServiceGetAll();
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<DisciplinaMinistrada> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPost(DisciplinaMinistrada_Input disciplinaMinistrada)
        {
            var test_coordenador = this.dbContext.DisciplinaMinistradas
                .Where(e => e.idTurma == disciplinaMinistrada.idTurma)
                .Where(e => e.idProfessor == disciplinaMinistrada.idProfessor)
                .Where(e => e.coordenador == true) .FirstOrDefault();
            if (test_coordenador != null)
            {
                var errorObj = new DuplicatedFieldError();
                errorObj.AddField("coordenador");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var disciplina = this.dbContext.Disciplinas.FirstOrDefault(e => e.id == disciplinaMinistrada.idDisciplina);
            var turma = this.dbContext.Turmas.FirstOrDefault(e => e.id == disciplinaMinistrada.idTurma);
            var professor = this.dbContext.Professores.FirstOrDefault(e => e.id == disciplinaMinistrada.idProfessor);
            if (disciplina == null || turma == null || professor == null)
            {
                var errorObj = new InvalidIdReferenceError();
                if (disciplina == null) errorObj.AddId("disciplina", disciplinaMinistrada.idDisciplina);
                if (turma == null) errorObj.AddId("turma", disciplinaMinistrada.idTurma);
                if (professor == null) errorObj.AddId("professor", disciplinaMinistrada.idProfessor);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePost(disciplinaMinistrada);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPut(int id, DisciplinaMinistrada_Input disciplinaMinistrada)
        {
            var test_coordenador = this.dbContext.DisciplinaMinistradas
                .Where(e => e.idTurma == disciplinaMinistrada.idTurma)
                .Where(e => e.idProfessor == disciplinaMinistrada.idProfessor)
                .Where(e => e.coordenador == true).FirstOrDefault();
            if (test_coordenador != null && disciplinaMinistrada.coordenador == test_coordenador.coordenador)
            {
                var errorObj = new DuplicatedFieldError();
                errorObj.AddField("coordenador");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.DisciplinaMinistradas.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");

            var disciplina = this.dbContext.Disciplinas.FirstOrDefault(e => e.id == disciplinaMinistrada.idDisciplina);
            var turma = this.dbContext.Turmas.FirstOrDefault(e => e.id == disciplinaMinistrada.idTurma);
            var professor = this.dbContext.Professores.FirstOrDefault(e => e.id == disciplinaMinistrada.idProfessor);
            if (disciplina == null || turma == null || professor == null)
            {
                var errorObj = new InvalidIdReferenceError();
                if (disciplina == null) errorObj.AddId("disciplina", disciplinaMinistrada.idDisciplina);
                if (turma == null) errorObj.AddId("turma", disciplinaMinistrada.idTurma);
                if (professor == null) errorObj.AddId("professor", disciplinaMinistrada.idProfessor);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePut(id, disciplinaMinistrada);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.DisciplinaMinistradas.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
