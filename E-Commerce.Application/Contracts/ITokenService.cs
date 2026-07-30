using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Contracts
{
    public interface ITokenService
    {
        public string CreateToken(string userId, string email, string userName, IEnumerable<string> roles);

      
    }
}
