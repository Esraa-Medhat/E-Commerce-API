using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager) : IAuthService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
           var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnAuthorizedException();
            var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!flag) throw new UnAuthorizedException();
            return new UserResultDto
            {
                DisplayName = user.DisplayName,
                Email= user.Email,
                Token ="TOKEN",
            };
          
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email,
                Email= registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,

            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if(! result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description);
                throw new ValidationException(errors);
            }
            return new UserResultDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "TOKEN",
            };
        }
    }
}
