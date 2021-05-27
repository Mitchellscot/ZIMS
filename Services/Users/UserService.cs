using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZIMS.Data.Entities;
using ZIMS.Helpers;
using ZIMS.Models.Authentication;
using ZIMS.Models.PasswordReset;

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
            new User { Id = 1, FirstName = "Mitchell", LastName = "Scott", Email = "mitchellscott@me.com", Password = "password", Role = Role.Manager },
            new User { Id = 2, FirstName = "Sarah", LastName = "Scott", Email = "sarahlscott@me.com", Password = "password", Role = Role.Guide }
        };

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);
            // return null if user not found
            if (user == null) return null;
            // authentication successful so generate jwt token
            var token = generateJwtToken(user);
            return new AuthenticateResponse(user, token);
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
                    new Claim(ClaimTypes.Name, user.Email.ToString()), 
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public void ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var user = _users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null) return;
            user.ResetToken = randomTokenString();
            var expires = DateTime.UtcNow.AddDays(1);

            //_users.Update(user);
            //_users.SaveChanges();
        }

        public void ResetPassword(ResetPasswordRequest model)
        {
            throw new NotImplementedException();
        }

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public User Update(User model)
        {
            var user = _users.SingleOrDefault(u => u.Id == model.Id);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Password = model.Password;
                user.Role = model.Role;
                user.ResetToken = user.ResetToken;
            }
            return user;
        }

        //placeholder for EF
        public int Commit()
        {
            return 0;
        }

        //not used yet
        public User Add(User model)
        {
            _users.Add(model);
            model.Id = _users.Max(u => u.Id) +1;
            return model;
        }

        //not used yet
        public User Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
            }
            return user;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }
    }
}
