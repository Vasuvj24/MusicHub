using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicHub.Infrastructure.Auth
{
    public sealed class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string CreateAccessToken(User user)
        {
            //configuration keys might be null somewhere so for that to tell that it will not be null and to avoid the can be null warning
            var issuer = _config["Jwt:Issuer"]!;
            var audience = _config["Jwt:Audience"]!;
            var key = _config["Jwt:Key"]!;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            //converts this key to bytes
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //adds signing algorithm
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //its object need to convert to token while returning
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
