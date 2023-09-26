using Microsoft.EntityFrameworkCore;
using API.DataBase;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Validations;
using API.Data.Models;

namespace API.Data.Identity
{
    public class AuthenticateService
    {
        private readonly DBContext context;
        private readonly IConfiguration configuration;

        public AuthenticateService(DBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public bool AutenticarPessoa(int id, string senha)
        {
            var pessoa = this.GetPessoa(id);
            if (pessoa == null || pessoa.senha != senha)
            {
                return false;
            }
            
            return true;
        }

        public string GerarToken(int id, string email, string cargo)
        {
            var claimsL = new List<Claim>();
            claimsL.Add(new Claim("id", id.ToString()));
            claimsL.Add(new Claim("email", email));
            claimsL.Add(new Claim("role", cargo));
            claimsL.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            var chaveSeguranca = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:secretKey"]));
            var credencial = new SigningCredentials(chaveSeguranca, SecurityAlgorithms.HmacSha256);
            var expiracao = DateTime.UtcNow.AddMinutes(10);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: this.configuration["Jwt:issuer"],
                audience: this.configuration["Jwt:audience"],
                claims: claimsL,
                expires: expiracao,
                signingCredentials: credencial
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Pessoa? GetPessoa(int id)
        {
            return this.context.Pessoas.FirstOrDefault(n => n.id == id);
        }
    }
}
