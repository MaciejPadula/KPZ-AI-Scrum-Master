using Artificial.Scrum.Master.User.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Artificial.Scrum.Master.User.Infrastructure.JWT
{
    internal interface IJwtAuthorizationService
    {
        string GenerateJwtToken(UserInfo userInfo);
    }
    internal class JwtAuthorizationService(IConfiguration _configuration) : IJwtAuthorizationService
    {
        public string GenerateJwtToken(UserInfo userInfo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("UserId", userInfo.UserId),
                new Claim("UserName", userInfo.UserName),
                new Claim("PhotoUrl", userInfo.PhotoUrl)
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token validity period
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
