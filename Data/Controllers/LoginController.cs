using API.DataBase;
using Microsoft.AspNetCore.Mvc;
using API.Data.Services;
using API.Data.ViewModels;
using API.Data.Errors;
using API.Data.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using API.Data.Models;
using System.IdentityModel.Tokens.Jwt;

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
        public ActionResult<dynamic> Login(Login login)
        {
            var pessoa = this.authenticate.GetPessoa(login.id);
            if (pessoa == null)
            {
                return BadRequest("id incorreto");
            }

            if (this.authenticate.AutenticarPessoa(login.id, login.senha) == false)
            {
                return BadRequest("senha incorreta.");
            }

            var token = this.authenticate.GerarToken(pessoa);
            var refreshToken = this.authenticate.GerarRefreshToken(pessoa);

            return Ok(new 
            {
                id = pessoa.id,
                email = pessoa.email,
                cargo = pessoa.cargo,
                token = token,
                refreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Refresh(Refresh refresh)
        {
            var pessoa = this.authenticate.GetPessoa(refresh.id);
            var valido = this.authenticate.ValidarRefreshToken(refresh.refreshToken, refresh.id);

            if (!valido)
            {
                BadRequest("Refresh token inválido ou expirado. Tente logar novamente \"api/login\".");
            }

            var newToken = this.authenticate.GerarToken(pessoa);

            return Ok(new
            {
                id = pessoa.id,
                email = pessoa.email,
                cargo = pessoa.cargo,
                token = newToken,
                valido = valido
            });
        }
    }
}
