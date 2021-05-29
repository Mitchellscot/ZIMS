﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
using BCryptNet = BCrypt.Net.BCrypt;

namespace ZIMS.Data.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IOptions<AppSettings> appSettings, IMapper mapper, AppDbContext context, ILogger<UserService> logger)
        {
            this._mapper = mapper;
            this._appSettings = appSettings.Value;
            this._context = context;
            this._logger = logger;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
        {
            //strange that I have to add this but whatever...
             var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, AuthenticateResponse>();
            });
            var MAPPER = config.CreateMapper();
            //hopefully one day I will be able to get rid of the above code.

            var user = await _context.Users.Where(u => u.Email.ToUpper() == model.Email.ToUpper()).FirstOrDefaultAsync<User>();
            if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash))
            {
                _logger.LogInformation("it's null");
                return null;
            }
            AuthenticateResponse response = MAPPER.Map<AuthenticateResponse>(user);
            response.Token = generateJwtToken(user);
            return response;
        }
        //Get a single user async
        public async Task<User> GetUserById(int id)
        {
            var query = _context.Users.Where(u => u.Id == id);
            if (query.Any())
            {
                return await query.FirstOrDefaultAsync();
            }
            return null;
        }
        //for use by internal methods... kind of redundant but it's not async.
        public User GetById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return null;
            return user;
        }
        //gets all users async
        public async Task<User[]> GetAllUsers()
        {
            //set up a data structure that can be queried
            IQueryable<User> query = _context.Users;
            //for any given user, order it by their last name
            query = query.OrderBy(u => u.LastName);
            //turn it back into an array
            return await query.ToArrayAsync();
        }


        public async Task<bool> Commit()
        {
            //only return if more than one row was affected
            return (await _context.SaveChangesAsync()) > 0;
        }

        //to be used in forgot password methods
        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
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

        //non async code:
        /*         public AuthenticateResponse Authenticate(AuthenticateRequest model)
                {
                    var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);
                    if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash)) return null;
                    var response = _mapper.Map<AuthenticateResponse>(user);
                    response.Token = generateJwtToken(user);
                    return response;
                } */

    }
}
