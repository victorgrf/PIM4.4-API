using Microsoft.EntityFrameworkCore;
using API.DataBase;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Validations;
using API.Data.Models;
using System.Security.Cryptography;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

        public bool AutenticarPessoa(int id, string senha)
        {
            var pessoa = this.GetPessoa(id);
            if (pessoa == null || pessoa.senha != senha)
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
                Expires = DateTime.UtcNow.AddMinutes(60),
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
                Expires = DateTime.UtcNow.AddMinutes(2),
                TokenType = "rt+jwt"
            });

            Console.WriteLine("SECURITY KEY: " + refreshToken.SecurityKey);
            return handler.WriteToken(refreshToken);
        }

        public bool ValidarRefreshToken(string refreshToken, int id)
        {
            var chaveSeguranca = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:secretKey"]));
            var handler = new JwtSecurityTokenHandler();
            try
            {
                handler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidIssuer = this.configuration["Jwt:issuer"],
                    ValidAudience = this.configuration["Jwt:audience"],
                    IssuerSigningKey = chaveSeguranca,
                }, out SecurityToken tokenValidado);

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
