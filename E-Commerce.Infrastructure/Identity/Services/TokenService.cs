using E_commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Identity.Services
{
    public class TokenService(IOptions<JWTSettings> jwtOptions) : ITokenService
    {
        private readonly JWTSettings _Settings = jwtOptions.Value;
        public string CreateToken(string userId, string email, string userName, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, userId),
                new (ClaimTypes.Email, email),
                new (ClaimTypes.Name, userName)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            if(string.IsNullOrEmpty(_Settings.SecretKey))
            {
                throw new InvalidOperationException("JWT SecretKey is Missing");

            }
            if (_Settings.SecretKey.Length < 32)
                throw new InvalidOperationException("JWT SecretKey Is Too Short");
            var Key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Settings.SecretKey));
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken
                (
                issuer: _Settings.Issuer,
                audience: _Settings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_Settings.ExpirationMinutes),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
    public class JWTSettings
    {
        public string SecretKey { get; init; } = default!;
        public string Issuer { get; init; }= default!;
        public string Audience { get; init; } = default!;
        public int ExpirationMinutes { get; init; } = 60;
    }
}
