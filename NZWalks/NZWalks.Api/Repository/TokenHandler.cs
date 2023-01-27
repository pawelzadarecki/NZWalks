using Microsoft.IdentityModel.Tokens;
using NZWalks.Api.Models.Domian;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.Api.Repository
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration configuration;

        public TokenHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> CreateTokenAsync(User user)
        {
            //Create claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.GivenName, user.UserName));
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
            claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress));

            user.Roles.ForEach ((role) =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            //get the key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["JWT:Issuer"],
                configuration["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

           return Task.FromResult( new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
