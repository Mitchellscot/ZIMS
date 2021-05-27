using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZIMS.Data.Entities;
using ZIMS.Models.Authentication;
using ZIMS.Models.PasswordReset;

namespace ZIMS.Data.Services.Users
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        void ForgotPassword(ForgotPasswordRequest model, string origin);
        void ResetPassword(ResetPasswordRequest model);
        User Update(User model);
        int Commit();
        User Add(User model);
        User Delete(int id);
        User GetById(int id);
    }
}
