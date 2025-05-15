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
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager,IConfiguration configuration) : IAuthService
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
                Token = await GenerateJwtTokenAsync(user),
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
                Token =await GenerateJwtTokenAsync(user),
            };
        }
        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var authClaim = new List<Claim>()
            {
               new Claim (ClaimTypes.Name,user.UserName),
               new Claim (ClaimTypes.Email,user.Email),

            };
            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role));

            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]));
            var token = new JwtSecurityToken(
               issuer: configuration["JwtOptions:Issuer"],
               audience: configuration["JwtOptions:Audience"],
               claims:authClaim,
               expires:DateTime.UtcNow.AddDays(double.Parse(configuration["JwtOptions:DurationInDays"])),
               signingCredentials:new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
                
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
