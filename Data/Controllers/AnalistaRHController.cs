using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public ActionResult<List<AnalistaRH>> HttpGetAll(string? nome)
        {
            var response = this.service.GetAllAnalistasRH(nome);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpGet("{id}")]
        public ActionResult<AnalistaRH> HttpGet(int id)
        {
            var response = this.service.GetAnalistaRH(id);
            if (response == null) return NotFound("Nenhum resultado obtido");
            return response;
        }

        [HttpPost]
        public IActionResult HttpPost(AnalistaRH_Input analistaRH)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == analistaRH.cpf).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == analistaRH.rg).FirstOrDefault();
            if (test_cpf != null || test_rg != null)
            {
                var str = "Já existe um cadastro com os seguintes itens: ";
                if (test_cpf != null) str = str + "cpf ";
                if (test_rg != null) str = str + "rg ";
                return Conflict(str);
            }

            this.service.AddAnalistaRH(analistaRH);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult HttpPut(int id, AnalistaRH_Input analistaRH)
        {
            var test_cpf = this.dbContext.Pessoas.Where(e => e.cpf == analistaRH.cpf).Where(e => e.id != id).FirstOrDefault();
            var test_rg = this.dbContext.Pessoas.Where(e => e.rg == analistaRH.rg).Where(e => e.id != id).FirstOrDefault();
            if ((test_cpf != null && analistaRH.cpf == test_cpf.cpf) || (test_rg != null && analistaRH.rg == test_rg.rg))
            {
                var str = "Já existe outro cadastro com os seguintes itens: ";
                if (test_cpf != null) str = str + "cpf ";
                if (test_rg != null) str = str + "rg ";
                return Conflict(str);
            }

            var table = this.dbContext.AnalistasRH.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.UpdateAnalistaRH(id, analistaRH);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult HttpDelete(int id)
        {
            var table = this.dbContext.AnalistasRH.Where(e => e.id == id).FirstOrDefault();
            if (table == null) return NotFound("Nenhuma tabela deste tipo de entidade e com este id foi encontrada no banco de dados");
            this.service.RemoveAnalistaRH(table);
            return Ok();
        }
    }
}
