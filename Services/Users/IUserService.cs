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
        //General
        Task<bool> Commit(); //save changes
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
        User GetById(int id);
        Task<User> GetUserById(int id);
        Task<User[]> GetAllUsers();
        //not implemented yet. Will have to create files for the Task<Response> like above.
/*      void ForgotPassword(ForgotPasswordRequest model, string origin);
        void ResetPassword(ResetPasswordRequest model);
        void UpdateUser(User model);
        void AddUser(User model);
        void DeleteUser(int id); */

    }
}
