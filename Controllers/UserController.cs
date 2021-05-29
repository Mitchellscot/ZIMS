using Microsoft.AspNetCore.Mvc;
using ZIMS.Data.Services.Users;
using ZIMS.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using ZIMS.Data.Entities;
using Microsoft.Extensions.Logging;
using ZIMS.Models.PasswordReset;
using AutoMapper;

namespace ZIMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        //forgot password route
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordRequest model)
        {
            _userService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok(new {message = "Please check your email for password reset instructions"});
        }
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordRequest model)
        {
            _userService.ResetPassword(model);
            return Ok(new {message = "Password reset successful, you can now login"});
        }
    }
}