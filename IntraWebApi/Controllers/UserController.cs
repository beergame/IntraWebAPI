using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntraWebApi.Models;
using IntraWebApi.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntraWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]Register userRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _userService.Create(userRegister.Civility, userRegister.FirstName, userRegister.LastName,
                userRegister.Username, userRegister.Password);

            return StatusCode(201);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]Authenticate userToAuthenticate)
        {
            if (string.IsNullOrEmpty(userToAuthenticate.Username) || string.IsNullOrEmpty(userToAuthenticate.Password))
                return BadRequest("Username and password required.");
            var result = await _userService.Authenticate(userToAuthenticate.Username, userToAuthenticate.Password);
            return Ok(result);
        }
    }
}