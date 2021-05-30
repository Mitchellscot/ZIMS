﻿using Microsoft.AspNetCore.Mvc;
using ZIMS.Data.Services.Users;
using ZIMS.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using ZIMS.Data.Entities;
using Microsoft.Extensions.Logging;
using ZIMS.Models.PasswordReset;
using AutoMapper;
using System.Threading.Tasks;

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
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            try
            {
                var response = await _userService.AuthenticateAsync(model);
                if (response == null){
                    return Unauthorized(new {message = "Email or Password incorrect"});
                }
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"HEY MITCH - ERROR AUTHENTICATING {ex.Message}");
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }
    }
}