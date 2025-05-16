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
using Microsoft.Extensions.Options;
using Shared.OrdersModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager,IOptions<JwtOptions> options,IMapper mapper) : IAuthService
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

            if (await CheckEmailExistsAsync(registerDto.Email))
            {
                throw new DuplicatedEmailBadRequestException(registerDto.Email);
            }

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

            var jwtOptions = options.Value;
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
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var token = new JwtSecurityToken(
               issuer: jwtOptions.Issuer,
               audience: jwtOptions.Audience,
               claims:authClaim,
               expires:DateTime.UtcNow.AddDays(jwtOptions.DurationInDays),
               signingCredentials:new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
                
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U=>U.Email == email);
            if (user is null) throw new UserNotFoundExceptions(email);
            var result = mapper.Map<AddressDto>(user.Address);
            return result;
            
        }

        public async Task<UserResultDto> GetCurrentUserAsync(string email)
        {
           var user = await userManager.FindByEmailAsync(email);
            if (user is null) throw new UserNotFoundExceptions(email);
            return new UserResultDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user),
            };
        }
        public async Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto address, string email)
        {
            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);
            if (user is null) throw new UserNotFoundExceptions(email);
           if(user.Address is not null)
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.City = address.City;
                user.Address.Country = address.Country;
                user.Address.Street = address.Street;
               
            }
            else
            {
                var addressResult = mapper.Map<Address>(address);
                user.Address = addressResult;
            }
            return address;
        }
    }
}
