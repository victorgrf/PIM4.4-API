using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly AlunoService service;

        public AlunoController(DBContext context, AlunoService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        public ActionResult<List<Aluno>> HttpGetAll(string? nome)
        {
            var response = this.service.GetAllAlunos(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        public ActionResult<Aluno> HttpGet(int id)
        {
            var response = this.service.GetAluno(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        public IActionResult HttpPost(Aluno_Input aluno)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == aluno.cpf).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == aluno.rg).FirstOrDefault();
            if (test_cpf != null || test_rg != null)
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.AddAluno(aluno);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult HttpPut(int id, Aluno_Input aluno)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == aluno.cpf).Where(e => e.id != id).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == aluno.rg).Where(e => e.id != id).FirstOrDefault();
            if ((test_cpf != null && aluno.cpf == test_cpf.cpf) || (test_rg != null && aluno.rg == test_rg.rg))
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.Alunos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.UpdateAluno(id, aluno);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Alunos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.RemoveAluno(table);
            return Ok();
        }
    }
}
