using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly ProfessorService service;

        public ProfessorController(DBContext context, ProfessorService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        public ActionResult<List<Professor>> HttpGetAll(string? nome)
        {
            var response = this.service.ServiceGetAll(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        public ActionResult<Professor> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        public IActionResult HttpPost(Professor_Input professor)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == professor.cpf).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == professor.rg).FirstOrDefault();
            if (test_cpf != null || test_rg != null)
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePost(professor);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult HttpPut(int id, Professor_Input professor)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == professor.cpf).Where(e => e.id != id).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == professor.rg).Where(e => e.id != id).FirstOrDefault();
            if ((test_cpf != null && professor.cpf == test_cpf.cpf) || (test_rg != null && professor.rg == test_rg.rg))
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.Professores.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServicePut(id, professor);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Professores.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
