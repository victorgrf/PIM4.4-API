using API.DataBase;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Data.Models;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace API.Data.Identity
{
    public class AuthenticateService
    {
        private readonly DBContext context;
        private readonly IConfiguration configuration;
        private SecurityKey securityKey { get; set; }

        public AuthenticateService(DBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public bool PrimeiroLogin(int id, string senha)
        {
            var pessoa = this.GetPessoa(id);
            if (pessoa != null && pessoa.senhaAlterada == false)
            {
                return true;
            }
            return false;
        }

        public bool AutenticarPessoa(int id, string senha)
        {
            var pessoa = this.GetPessoa(id);
            var hash = Criptografia.CriptografarSenha(senha);
            if (pessoa == null || pessoa.senha != hash)
            {
                return false;
            }
            
            return true;
        }

        private ClaimsIdentity GerarClaims(int id, string? email, string? cargo)
        {
            var claimsIdentity = new ClaimsIdentity();
            if (email != null) claimsIdentity.AddClaim(new Claim("email", email));
            if (cargo != null) claimsIdentity.AddClaim(new Claim("role", cargo));
            claimsIdentity.AddClaim(new Claim("id", id.ToString()));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            return claimsIdentity;
        }

        public string GerarToken(Pessoa pessoa)
        {
            var chaveSeguranca = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:secretKey"]));
            var claimsIdentity = this.GerarClaims(pessoa.id, pessoa.email, pessoa.cargo);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = this.configuration["Jwt:issuer"],
                Audience = this.configuration["Jwt:audience"],
                SigningCredentials = new SigningCredentials(chaveSeguranca, SecurityAlgorithms.HmacSha256),
                Subject = claimsIdentity,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddSeconds(0),
                IssuedAt = DateTime.UtcNow,
                TokenType = "at+jwt"
            });

            return handler.WriteToken(token);
        }

        public string GerarRefreshToken(Pessoa pessoa)
        {
            var chaveSeguranca = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:secretKey"]));
            var claimsIdentity = this.GerarClaims(pessoa.id, null, null);

            var handler = new JwtSecurityTokenHandler();
            var refreshToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = this.configuration["Jwt:issuer"],
                Audience = this.configuration["Jwt:audience"],
                SigningCredentials = new SigningCredentials(chaveSeguranca, SecurityAlgorithms.HmacSha256),
                Subject = claimsIdentity,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
                TokenType = "rt+jwt"
            });

            return handler.WriteToken(refreshToken);
        }

        public bool ValidarRefreshToken(string token, string refreshToken, int id)
        {
            var chaveSeguranca = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:secretKey"]));
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = this.configuration["Jwt:issuer"],
                ValidAudience = this.configuration["Jwt:audience"],
                ValidateLifetime = true,
                IssuerSigningKey = chaveSeguranca
            };
            try
            {
                handler.ValidateToken(token, parameters, out SecurityToken tokenValidado);
            }
            catch (SecurityTokenExpiredException)
            {
                Console.WriteLine("token expirado");
            }
            catch
            {
                Console.WriteLine("token inválido");
                return false;
            }
            
            try
            {
                handler.ValidateToken(refreshToken, parameters, out SecurityToken refreashTokenValidado);
                return true;
            }
            catch
            {
                return false;
            }
        }
        

        public Pessoa? GetPessoa(int id)
        {
            return this.context.Pessoas.FirstOrDefault(n => n.id == id);
        }
    }
}
