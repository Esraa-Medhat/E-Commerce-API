using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace presentation
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController(IServiceManager serviceManager):ControllerBase
    {
        [HttpPost("login")] // api/Auth/login
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(result);

        }
        [HttpPost("register")] // api/Auth/login
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await serviceManager.AuthService.RegisterAsync(registerDto);
            return Ok(result);

        }
    }
}
