using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZIMS.Data.Entities;
using ZIMS.Helpers;
using ZIMS.Models.Authenticate;

namespace ZIMS.Data.Services.Users
{
    public class UserService : IUserService
    { 
        private readonly AppSettings _appSettings;
        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        //TODO: change to database users when DB is set up
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Mitchell", LastName = "Scott", Username = "mitch", Password = "password", Role = Role.Manager },
            new User { Id = 2, FirstName = "Sarah", LastName = "Scott", Username = "sarah", Password = "password", Role = Role.Guide }
        };

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
            // return null if user not found
            if (user == null) return null;
            // authentication successful so generate jwt token
            var token = generateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username.ToString()), 
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }
        public User GetByName(string name)
        {
            return _users.FirstOrDefault(x => x.Username == name);
        }
    }
}
