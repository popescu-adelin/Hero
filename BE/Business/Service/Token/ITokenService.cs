using DataLogic.Entities;
using DataLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.Token
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
