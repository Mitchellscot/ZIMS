using AutoMapper;
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
using BCryptNet = BCrypt.Net.BCrypt;

namespace ZIMS.Data.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;

        public UserService(IOptions<AppSettings> appSettings, IMapper mapper, AppDbContext context)
        {
            this._mapper = mapper;
            this._appSettings = appSettings.Value;
            this._context = context;
        }


        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
             var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);
            if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash)) return null;
            var token = generateJwtToken(user);
            var response = _mapper.Map<AuthenticateResponse>(user);
            return response;
        }

        private string generateJwtToken(User user)
        {
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
            /*var user = _users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null) return;
            user.ResetToken = randomTokenString();
            var expires = DateTime.UtcNow.AddDays(1);

            //_users.Update(user);
            //_users.SaveChanges(); */
            throw new NotImplementedException();
        } 

        public void ResetPassword(ResetPasswordRequest model)
        {
            throw new NotImplementedException();
        }

        public User Update(User model)
        {
            throw new NotImplementedException();
        }

        //not used yet
        public void Add(User model)
        {
           throw new NotImplementedException();
        }

        //not used yet
        public void Delete(int id)
        {
            var user = GetById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        //to be used in forgot password methods
        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

    }
}
