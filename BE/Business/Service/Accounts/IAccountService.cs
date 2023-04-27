using DataLogic.Mappings.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.Accounts
{
    public interface IAccountService
    {
        Task<UserDto> Login(LoginDto loginDto);
    }
}
