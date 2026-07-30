using E_commerce.Application.Common;
using E_commerce.Application.DTOS.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Contracts
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindEmailAsync(string email,CancellationToken ct=default);
        Task<Result<bool>>CheckPasswordAsync(string email,string password,CancellationToken ct=default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto,CancellationToken ct=default);
    }
}
