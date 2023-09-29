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
    public class DisciplinaCursadaController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly DisciplinaCursadaService service;

        public DisciplinaCursadaController(DBContext context, DisciplinaCursadaService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<List<DisciplinaCursada>> HttpGetAll()
        {
            var response = this.service.ServiceGetAll();
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<DisciplinaCursada> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPost([FromForm] DisciplinaCursada_Input disciplinaCursada)
        {
            var disciplina = this.dbContext.Disciplinas.FirstOrDefault(e => e.id == disciplinaCursada.idDisciplina);
            var cursoMatriculado = this.dbContext.CursoMatriculados.FirstOrDefault(e => e.id == disciplinaCursada.idCursoMatriculado);
            if (disciplina == null || cursoMatriculado == null)
            {
                var errorObj = new InvalidIdReferenceError();
                if (disciplina == null) errorObj.AddId("disciplina", disciplinaCursada.idDisciplina);
                if (cursoMatriculado == null) errorObj.AddId("cursoMatriculado", disciplinaCursada.idCursoMatriculado);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var curso = this.dbContext.Cursos.FirstOrDefault(e => e.id == cursoMatriculado.idCurso);
            var curso_discplinas = this.dbContext.Curso_Disciplinas
            .Where(n => n.idCurso == cursoMatriculado.idCurso)
            .Where(n => n.idDisciplina == disciplinaCursada.idDisciplina).FirstOrDefault();
            var relacaoExiste = false;
            if (curso_discplinas != null) relacaoExiste = true;
            if (!relacaoExiste)
            {
                var errorObj = new NotRelatedError(disciplinaCursada.idDisciplina, cursoMatriculado.idCurso, "Disciplina não faz parte do curso.");
               return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePost(disciplinaCursada);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPut(int id, [FromForm] DisciplinaCursada_Input disciplinaCursada)
        {
            var table = this.dbContext.DisciplinaCursadas.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");

            var disciplina = this.dbContext.Disciplinas.FirstOrDefault(e => e.id == disciplinaCursada.idDisciplina);
            var cursoMatriculado = this.dbContext.CursoMatriculados.FirstOrDefault(e => e.id == disciplinaCursada.idCursoMatriculado);
            if (disciplina == null || cursoMatriculado == null)
            {
                var errorObj = new InvalidIdReferenceError();
                if (disciplina == null) errorObj.AddId("disciplina", disciplinaCursada.idDisciplina);
                if (cursoMatriculado == null) errorObj.AddId("cursoMatriculado", disciplinaCursada.idCursoMatriculado);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var curso = this.dbContext.Cursos.FirstOrDefault(e => e.id == cursoMatriculado.idCurso);
            var curso_discplinas = this.dbContext.Curso_Disciplinas
            .Where(n => n.idCurso == cursoMatriculado.idCurso)
            .Where(n => n.idDisciplina == disciplinaCursada.idDisciplina).FirstOrDefault();
            var relacaoExiste = false;
            if (curso_discplinas != null) relacaoExiste = true;
            if (!relacaoExiste)
            {
                var errorObj = new NotRelatedError(disciplinaCursada.idDisciplina, cursoMatriculado.idCurso, "Disciplina não faz parte do curso.");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePut(id, disciplinaCursada);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.DisciplinaCursadas.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }

        [HttpPut("media/{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public IActionResult CalcularMedia(int id)
        {
            var disciplinaCursada = this.dbContext.DisciplinaCursadas.FirstOrDefault(e => e.id == id);
            if (disciplinaCursada == null)
            {
                var errorObj = new InvalidIdReferenceError();
                errorObj.AddId("disciplinaCursada", disciplinaCursada.idCursoMatriculado);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            if (disciplinaCursada.prova1 == null || disciplinaCursada.prova2 == null || disciplinaCursada.trabalho == null)
            {
                var errorObj = new ImpossibleToCalcError();
                if (disciplinaCursada.prova1 == null) errorObj.AddVariavel("disciplinaCursadas.prova1");
                if (disciplinaCursada.prova2 == null) errorObj.AddVariavel("disciplinaCursadas.prova2");
                if (disciplinaCursada.trabalho == null) errorObj.AddVariavel("disciplinaCursadas.trabalho");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var media = this.service.ServiceMedia(id);
            return StatusCode(StatusCodes.Status200OK, new
            {
                status = StatusCodes.Status200OK,
                title = "Calculado",
                media = media
            });
        }

        [HttpPut("frequencia/{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public IActionResult CalcularFrequencia(int id)
        {
            var disciplinaCursada = this.dbContext.DisciplinaCursadas.FirstOrDefault(e => e.id == id);
            if (disciplinaCursada == null)
            {
                var errorObj = new InvalidIdReferenceError();
                errorObj.AddId("disciplinaCursada", disciplinaCursada.idCursoMatriculado);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            if (disciplinaCursada.faltas == null)
            {
                var errorObj = new ImpossibleToCalcError();
                errorObj.AddVariavel("disciplinaCursadas.faltas");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var frequencia = this.service.ServiceFrequencia(id);
            return StatusCode(StatusCodes.Status200OK, new
            {
                status = StatusCodes.Status200OK,
                title = "Calculado",
                frequencia = frequencia
            });
        }

        [HttpPut("situacao{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public IActionResult CalcularSituacao(int id)
        {
            var disciplinaCursada = this.dbContext.DisciplinaCursadas.FirstOrDefault(e => e.id == id);
            if (disciplinaCursada == null)
            {
                var errorObj = new InvalidIdReferenceError();
                errorObj.AddId("disciplinaCursada", disciplinaCursada.idCursoMatriculado);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var situacao = this.service.ServiceSituacao(id);
            return StatusCode(StatusCodes.Status200OK, new
            {
                status = StatusCodes.Status200OK,
                title = "Calculado",
                media = situacao
            });
        }
    }
}
