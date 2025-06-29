﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using NoventiqAssignment.DB.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoventiqAssignment.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration Configuration;

        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string CreateAccessToken(ApplicationUser user, IList<string> userRoles)
        {
            string TokenKey = Configuration.GetSection("Token:Key").Value ?? string.Empty;

            var claims = GetBasicClaims(user, userRoles);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey));


            SigningCredentials creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDiscriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDiscriptor);


            return tokenHandler.WriteToken(token);
        }
        public string CreateRefreshToken(string userId)
        {
            string TokenKey = Configuration.GetSection("Token:Key").Value ?? string.Empty;

            Claim[] claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier , userId.ToString(),"nameid"),
                    new Claim(ClaimTypes.Role , "RefresfToken"),

            };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey));

            SigningCredentials creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDiscriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.Now.AddDays(4),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDiscriptor);


            return tokenHandler.WriteToken(token);
        }
        private List<Claim> GetBasicClaims(ApplicationUser user, IList<string> userRoles)
        {


            var userClaims = new List<Claim>()
            {
                    new Claim(ClaimTypes.NameIdentifier , user.Id.ToString(),"id"),
                     new Claim(ClaimTypes.Role , userRoles[0])

            };
            return userClaims;
        }
    }
}
