using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;
using API.Data.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace API.Data.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly SecretarioService service;
        private readonly AuthenticateService authenticate;

        public LoginController(DBContext context, SecretarioService service, IConfiguration configuration)
        {
            this.dbContext = context;
            this.service = service;
            this.authenticate = new AuthenticateService(this.dbContext, configuration);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<dynamic> HttpGetAll(Login login)
        {
            if (this.authenticate.AutenticarPessoa(login.id, login.senha) == false)
            {
                return BadRequest("id e/ou senha incorreto(s).");
            }

            var pessoa = this.authenticate.GetPessoa(login.id);
            var token = this.authenticate.GerarToken(pessoa.id, pessoa.email, pessoa.cargo);
            return Ok(new 
            {
                id = pessoa.id,
                email = pessoa.email,
                cargo = pessoa.cargo,
                token = token
            });
        }
    }
}
