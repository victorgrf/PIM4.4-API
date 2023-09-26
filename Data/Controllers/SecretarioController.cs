using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class SecretarioController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly SecretarioService service;

        public SecretarioController(DBContext context, SecretarioService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        public ActionResult<List<Secretario>> HttpGetAll(string? nome)
        {
            var response = this.service.ServiceGetAll(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        public ActionResult<Secretario> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        public IActionResult HttpPost(Secretario_Input secretario)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == secretario.cpf).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == secretario.rg).FirstOrDefault();
            var test_email = this.dbContext.Pessoas.Where(e => e.email == secretario.email).FirstOrDefault();
            if (test_cpf != null || test_rg != null || test_email != null)
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                if (test_email != null) errorObj.AddField("email");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePost(secretario);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult HttpPut(int id, Secretario_Input secretario)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == secretario.cpf).Where(e => e.id != id).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == secretario.rg).Where(e => e.id != id).FirstOrDefault();
            var test_email = this.dbContext.Pessoas.Where(e => e.email == secretario.email).Where(e => e.id != id).FirstOrDefault();
            if ((test_cpf != null && secretario.cpf == test_cpf.cpf) ||
                (test_rg != null && secretario.rg == test_rg.rg) ||
                (test_email != null && secretario.email == test_email.email))
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.Secretarios.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServicePut(id, secretario);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.Secretarios.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
