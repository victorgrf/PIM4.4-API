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
        private readonly AnalistaRHService service;
        private readonly AuthenticateService authenticate;

        public LoginController(DBContext context, AnalistaRHService service, IConfiguration configuration)
        {
            this.dbContext = context;
            this.service = service;
            this.authenticate = new AuthenticateService(this.dbContext, configuration);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<dynamic> Login([FromQuery] Login login)
        {
            var pessoa = this.authenticate.GetPessoa(login.id);
            if (pessoa == null)
            {
                return Unauthorized("id incorreto");
            }

            if (this.authenticate.PrimeiroLogin(login.id, login.senha))
            {
                return StatusCode(StatusCodes.Status426UpgradeRequired, new
                {
                    title = "Primeiro Login.",
                    description = "Para efetuar o primeiro login em uma conta, é necessário alterar a senha inicial que é gerada automáticamente pelo sistema por uma de preferência do usuário. Para efetuar essa ação, utilize a rota api/login/mudarsenha.",
                    status = StatusCodes.Status426UpgradeRequired
                });
            }

            if (this.authenticate.AutenticarPessoa(login.id, login.senha) == false)
            {
                return Unauthorized("senha incorreta.");
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
        public async Task<ActionResult<dynamic>> Refresh([FromForm] Refresh refresh)
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

        [HttpPost("startup")]
        [AllowAnonymous]
        public ActionResult<dynamic> Startup([FromForm] ViewModels.AnalistaRH_Input analistaRH)
        {
            var test = this.dbContext.Pessoas.FirstOrDefault();
            if (test != null)
            {
                return BadRequest("O starup já foi feito anteriormente.");
            }

            this.service.ServicePost(analistaRH);
            return Ok();
        }

        [HttpPut("mudarsenha")]
        public ActionResult<dynamic> MudarSenha([FromForm] ViewModels.MudarSenha mudarSenha)
        {
            if (mudarSenha.senhaNova == mudarSenha.senhaAntiga)
            {
                return BadRequest("A nova senha não pode ser igual a anterior");
            }

            var pessoa = this.authenticate.GetPessoa(mudarSenha.id);
            if (pessoa == null)
            {
                return Unauthorized("id incorreto");
            }

            if (this.authenticate.AutenticarPessoa(mudarSenha.id, mudarSenha.senhaAntiga) == false)
            {
                return Unauthorized("senha incorreta.");
            }

            pessoa.senha = Criptografia.CriptografarSenha(mudarSenha.senhaNova);
            this.dbContext.SaveChanges();

            return Ok();
        }
    }
}
