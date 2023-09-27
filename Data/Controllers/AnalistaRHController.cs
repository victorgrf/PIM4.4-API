using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using API.Data.Errors;
using API.Data.Identity;
using Microsoft.AspNetCore.Authorization;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AnalistaRHController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly AnalistaRHService service;

        public AnalistaRHController(DBContext context, AnalistaRHService service)
        {
            this.dbContext = context;
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.AnalistaRH)]
        public ActionResult<List<AnalistaRH>> HttpGetAll(string? nome)
        {
            var response = this.service.ServiceGetAll(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.AnalistaRH)]
        public ActionResult<AnalistaRH> HttpGet(int id)
        {
            var response = this.service.ServiceGet(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        [Authorize(Roles = Roles.AnalistaRH)]
        public IActionResult HttpPost([FromForm] AnalistaRH_Input analistaRH)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == analistaRH.cpf).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == analistaRH.rg).FirstOrDefault();
            var test_email = this.dbContext.Pessoas.Where(e => e.email == analistaRH.email).FirstOrDefault();
            if (test_cpf != null || test_rg != null || test_email != null)
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                if (test_email != null) errorObj.AddField("email");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            this.service.ServicePost(analistaRH);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.AnalistaRH)]
        public IActionResult HttpPut(int id, [FromForm] AnalistaRH_Input analistaRH)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == analistaRH.cpf).Where(e => e.id != id).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == analistaRH.rg).Where(e => e.id != id).FirstOrDefault();
            var test_email = this.dbContext.Pessoas.Where(e => e.email == analistaRH.email).Where(e => e.id != id).FirstOrDefault();
            if ((test_cpf != null && analistaRH.cpf == test_cpf.cpf) ||
                (test_rg != null && analistaRH.rg == test_rg.rg) ||
                (test_email != null && analistaRH.email == test_email.email))
            {
                var errorObj = new DuplicatedFieldError();
                if (test_cpf != null) errorObj.AddField("cpf");
                if (test_rg != null) errorObj.AddField("rg");
                if (test_email != null) errorObj.AddField("email");
                return StatusCode(errorObj.GetStatusCode(), errorObj);
            }

            var table = this.dbContext.AnalistasRH.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServicePut(id, analistaRH);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.AnalistaRH)]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.AnalistasRH.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.ServiceDelete(table);
            return Ok();
        }
    }
}
