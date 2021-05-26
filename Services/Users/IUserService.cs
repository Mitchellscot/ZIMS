using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZIMS.Data.Entities;
using ZIMS.Models.Authenticate;

namespace ZIMS.Data.Services.Users
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByName(string name);
    }
}
