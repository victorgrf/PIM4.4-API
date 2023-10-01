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
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Cursos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");

            var turma = this.dbContext.Turmas.Where(e => e.idCurso == id).ToList();
            var cursoMatriculado = this.dbContext.CursoMatriculados.Where(e => e.idTurma == id).ToList();
            if (turma.Count > 0 || cursoMatriculado.Count > 0)
            {
                var errorObj = new RelatedTableError();

                if (turma.Count > 0)
                {
                    var ids = new List<int>();
                    foreach (var t in turma) ids.Add(t.id);
                    errorObj.AddTable("turma", ids);
                }

                if (cursoMatriculado.Count > 0)
                {
                    var ids = new List<int>();
                    foreach (var t in cursoMatriculado) ids.Add(t.id);
                    errorObj.AddTable("cursoMatriculado", ids);
                }


                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServiceDelete(table);
            return Ok();
        }

        [HttpPost("disciplina")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult AddDisciplina(Curso_Disciplina_Input curso_disciplina)
        {
            var curso = this.dbContext.Cursos.FirstOrDefault(e => e.id == curso_disciplina.idCurso);
            var disciplina = this.dbContext.Disciplinas.FirstOrDefault(e => e.id == curso_disciplina.idDisciplina);
            if (disciplina == null || curso == null)
            {
                var errorObj = new InvalidIdReferenceError();
                if (disciplina == null) errorObj.AddId("disciplina", curso_disciplina.idDisciplina);
                if (curso == null) errorObj.AddId("curso", curso_disciplina.idCurso);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var exists = this.dbContext.Curso_Disciplinas
            .Where(e => e.idCurso == curso_disciplina.idCurso)
            .FirstOrDefault(e => e.idDisciplina == curso_disciplina.idDisciplina);
            if (exists != null)
            {
                var errorObj = new DuplicatedFieldError();
                errorObj.AddField("disciplina");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServiceAddDisciplina(curso_disciplina);
            return Ok();
        }

        [HttpDelete("disciplina")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult RemoveDisciplina(Curso_Disciplina_Input curso_disciplina)
        {
            var curso = this.dbContext.Cursos.FirstOrDefault(e => e.id == curso_disciplina.idCurso);
            var disciplina = this.dbContext.Disciplinas.FirstOrDefault(e => e.id == curso_disciplina.idDisciplina);
            if (disciplina == null || curso == null)
            {
                var errorObj = new InvalidIdReferenceError();
                if (disciplina == null) errorObj.AddId("disciplina", curso_disciplina.idDisciplina);
                if (curso == null) errorObj.AddId("curso", curso_disciplina.idCurso);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var exists = this.dbContext.Curso_Disciplinas
            .Where(e => e.idCurso == curso_disciplina.idCurso)
            .FirstOrDefault(e => e.idDisciplina == curso_disciplina.idDisciplina);
            if (exists == null)
            {
                var errorObj = new NotRelatedError(curso_disciplina.idDisciplina, curso_disciplina.idCurso, "Disciplina não faz parte do curso");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServiceRemoveDisciplina(exists);
            return Ok();
        }
    }
}
