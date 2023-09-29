using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;
using Microsoft.AspNetCore.Authorization;
using API.Data.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CursoMatriculadoController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly CursoMatriculadoService service;
        private readonly DisciplinaCursadaService disciplinaCursadaService;

        public CursoMatriculadoController(DBContext context, CursoMatriculadoService service, DisciplinaCursadaService disciplinaCursadaService)
        {
            this.dbContext = context;
            this.service = service;
            this.disciplinaCursadaService = disciplinaCursadaService;
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

            var cursoMatriculados = this.dbContext.CursoMatriculados.Where(e => e.idTurma == cursoMatriculado.idTurma).ToList();
            if (cursoMatriculados.Count >= 75)
            {
                return BadRequest("Limite de alunos nesta turma já atingido");
            }

            else if (cursoMatriculados.Count > 0)
            {
                var jaNaTurma = false;
                foreach (var cm in cursoMatriculados)
                {
                    if (cm.idAluno == cursoMatriculado.idAluno)
                    {
                        jaNaTurma = true;
                    }
                }

                if (jaNaTurma)
                {
                    return BadRequest("Este aluno já faz parte desta turma");
                }
            }

            if (turma.idCurso != cursoMatriculado.idCurso)
            {
                var errorObj = new NotRelatedError(cursoMatriculado.idCurso, turma.idCurso, "Turma não faz este curso");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePost(cursoMatriculado);

            var cursoMatriculadoCadastrado = this.dbContext.CursoMatriculados
            .Where(n => n.idAluno == cursoMatriculado.idAluno)
            .Where(n => n.idCurso == cursoMatriculado.idCurso)
            .Where(n => n.idTurma == cursoMatriculado.idTurma).FirstOrDefault();

            var curso_disciplinas = this.dbContext.Curso_Disciplinas.Where(n => n.idCurso == cursoMatriculado.idCurso).ToList();
            foreach (var c_d in curso_disciplinas)
            {
                var c_dVM = new ViewModels.DisciplinaCursada_Input
                {
                    idDisciplina = c_d.idDisciplina,
                    idCursoMatriculado = cursoMatriculadoCadastrado.id,
                    faltas = null,
                    prova1 = null,
                    prova2 = null,
                    trabalho = null
                };
                this.disciplinaCursadaService.ServicePost(c_dVM);
            }
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

            if (table.idAluno != cursoMatriculado.idAluno)
            {
                var cursoMatriculados = this.dbContext.CursoMatriculados.Where(e => e.idTurma == cursoMatriculado.idTurma).ToList();
                if (cursoMatriculados.Count >= 75)
                {
                    return BadRequest("Limite de alunos nesta turma já atingido");
                }

                else if (cursoMatriculados.Count > 0)
                {
                    var jaNaTurma = false;
                    foreach (var cm in cursoMatriculados)
                    {
                        if (cm.idAluno == cursoMatriculado.idAluno)
                        {
                            jaNaTurma = true;
                        }
                    }

                    if (jaNaTurma)
                    {
                        return BadRequest("Este aluno já faz parte desta turma");
                    }
                }
            }

            if (turma.idCurso != cursoMatriculado.idCurso)
            {
                var errorObj = new NotRelatedError(cursoMatriculado.idCurso, turma.idCurso, "Turma não faz este curso");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            // AQUI

            var disciplinaCursadas = this.dbContext.DisciplinaCursadas.Where(e => e.idCursoMatriculado == id).ToList();
            foreach (var dc in disciplinaCursadas)
            {
                this.disciplinaCursadaService.ServiceDelete(dc);
            }

            if (table.idCurso != cursoMatriculado.idCurso)
            {
                var curso_disciplinas = this.dbContext.Curso_Disciplinas.Where(n => n.idCurso == cursoMatriculado.idCurso).ToList();
                foreach (var c_d in curso_disciplinas)
                {
                    var c_dVM = new ViewModels.DisciplinaCursada_Input
                    {
                        idDisciplina = c_d.idDisciplina,
                        idCursoMatriculado = table.id,
                        faltas = null,
                        prova1 = null,
                        prova2 = null,
                        trabalho = null
                    };
                    this.disciplinaCursadaService.ServicePost(c_dVM);
                }
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

            var disciplinaCursadas = this.dbContext.DisciplinaCursadas.Where(e => e.idCursoMatriculado == id).ToList();
            //if (disciplinaCursadas.Count > 0)
            //{
            //    var errorObj = new RelatedTableError();
            //    var ids = new List<int>();
            //    foreach (var t in disciplinaCursadas) ids.Add(t.id);
            //    errorObj.AddTable("disciplinaCursada", ids);

            //    return StatusCode(errorObj.GetStatusCode(), errorObj);
            //}

            foreach (var dc in disciplinaCursadas)
            {
                this.disciplinaCursadaService.ServiceDelete(dc);
            }

            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
