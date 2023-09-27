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
    public class CursoMatriculadoController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly CursoMatriculadoService service;

        public CursoMatriculadoController(DBContext context, CursoMatriculadoService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<List<CursoMatriculado>> HttpGetAll()
        {
            var response = this.service.ServiceGetAll();
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Secretario + "," + Roles.Professor + "," + Roles.Aluno)]
        public ActionResult<CursoMatriculado> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPost([FromForm] CursoMatriculado_Input cursoMatriculado)
        {
            var curso = this.dbContext.Cursos.FirstOrDefault(e => e.id == cursoMatriculado.idCurso);
            var turma = this.dbContext.Turmas.FirstOrDefault(e => e.id == cursoMatriculado.idTurma);
            var aluno = this.dbContext.Alunos.FirstOrDefault(e => e.id == cursoMatriculado.idAluno);
            if (curso == null || turma == null || aluno == null)
            {
                var errorObj = new InvalidIdReferenceError();
                if (curso == null) errorObj.AddId("curso", cursoMatriculado.idCurso);
                if (turma == null) errorObj.AddId("turma", cursoMatriculado.idTurma);
                if (aluno == null) errorObj.AddId("aluno", cursoMatriculado.idAluno);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            if (turma.idCurso != cursoMatriculado.idCurso)
            {
                var errorObj = new NotRelatedError(cursoMatriculado.idCurso, turma.idCurso);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePost(cursoMatriculado);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpPut(int id, [FromForm] CursoMatriculado_Input cursoMatriculado)
        {
            var table = this.dbContext.CursoMatriculados.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");

            var curso = this.dbContext.Cursos.FirstOrDefault(e => e.id == cursoMatriculado.idCurso);
            var turma = this.dbContext.Turmas.FirstOrDefault(e => e.id == cursoMatriculado.idTurma);
            var aluno = this.dbContext.Alunos.FirstOrDefault(e => e.id == cursoMatriculado.idAluno);
            if (curso == null || turma == null || aluno == null)
            {
                var errorObj = new InvalidIdReferenceError();
                if (curso == null) errorObj.AddId("curso", cursoMatriculado.idCurso);
                if (turma == null) errorObj.AddId("turma", cursoMatriculado.idTurma);
                if (aluno == null) errorObj.AddId("aluno", cursoMatriculado.idAluno);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            if (turma.idCurso != cursoMatriculado.idCurso)
            {
                var errorObj = new NotRelatedError(cursoMatriculado.idCurso, turma.idCurso);
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePut(id, cursoMatriculado);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Secretario)]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.CursoMatriculados.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
