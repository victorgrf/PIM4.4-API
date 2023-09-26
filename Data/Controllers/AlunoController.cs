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
            var response = this.service.ServiceGetAll(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        public ActionResult<Aluno> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        public IActionResult HttpPost(Aluno_Input aluno)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == aluno.cpf).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == aluno.rg).FirstOrDefault();
            var test_email = this.dbContext.Pessoas.Where(e => e.email == aluno.email).FirstOrDefault();
            if (test_cpf != null || test_rg != null || test_email != null)
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                if (test_email != null) errorObj.AddField("email");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePost(aluno);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult HttpPut(int id, Aluno_Input aluno)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == aluno.cpf).Where(e => e.id != id).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == aluno.rg).Where(e => e.id != id).FirstOrDefault();
            var test_email = this.dbContext.Pessoas.Where(e => e.email == aluno.email).Where(e => e.id != id).FirstOrDefault();
            if ((test_cpf != null && aluno.cpf == test_cpf.cpf) || 
                (test_rg != null && aluno.rg == test_rg.rg) || 
                (test_email != null && aluno.email == test_email.email))
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                if (test_email != null) errorObj.AddField("email");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.Alunos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServicePut(id, aluno);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Alunos.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
