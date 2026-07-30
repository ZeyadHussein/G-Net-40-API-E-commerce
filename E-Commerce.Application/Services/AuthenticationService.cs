using E_commerce.Application.Common;
using E_commerce.Application.Contracts;
using E_commerce.Application.DTOS.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }
        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            //get user by email
            var userResult=await _identityService.FindEmailAsync(loginDto.Email, ct);
            if(!userResult.IsSuccess)
                return Result<UserDto>.Fail(userResult.Errors);

            //check password
            var passwordResult=await _identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password, ct);
            if(!passwordResult.IsSuccess)
                return Result<UserDto>.Fail(Error.Unauthorized("InValid Email or Password"));
     

            return new UserDto
            {
                Email = userResult.data.Email,
                DisplayName = userResult.data.DisplayName,
                Token="Token"
               
            };
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var result = await _identityService.CreateUserAsync(registerDto, ct);
            if (!result.IsSuccess || result.data is null)
            {
                return Result<UserDto>.Fail(result.Errors);
            }
            
                return new UserDto
                {
                    Email = result.data.Email,
                    DisplayName = result.data.DisplayName,
                    Token = "Token"
                };
        }
    }
}
