using Microsoft.AspNetCore.Mvc;
using ZIMS.Data.Services.Users;
using ZIMS.Models.Authenticate;
using Microsoft.AspNetCore.Authorization;
using ZIMS.Data.Entities;
using Microsoft.Extensions.Logging;

namespace ZIMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this._logger = logger;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize(Roles = Role.Manager)]
        [HttpGet("manager")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("you are a admin");
            var users = _userService.GetAll();
            return Ok(users);
        }

        [Authorize(Roles = Role.Guide)]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            _logger.LogInformation("you are a user");
            var users = _userService.GetAll();
            return Ok(users);
        }
        [Authorize]
        [HttpGet("name/{name}")]
        public IActionResult GetUserInfo(string name)
        {
            var user = _userService.GetByName(name);
            if (!User.IsInRole(Role.Manager)) {
                _logger.LogInformation("you are a user");
                return Ok(user);
            }
            if (!User.IsInRole(Role.Guide))
            {
                _logger.LogInformation("you are a admin");
                return Ok(user);
            }

            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Role.Guide)]
        public IActionResult GetById(int id)
        {
            // only allow admins to access other user records
            if (!User.IsInRole(Role.Manager))
                return Forbid();

            var user = _userService.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(user);

        }
    }
}