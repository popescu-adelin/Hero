using DataLogic.Mappings.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.Users
{
    public interface IUserService
    {
        Task<IEnumerable<MemberDto>> GetUsers();

        Task<MemberDto> GetUser(string email);

        Task<int> AddUser(NewMemberDto newMember);

        Task<int> UpdatePassword(PasswordUpdateDto passwordUpdate, ClaimsPrincipal claimedUser);

        Task<bool> UpdateUser(MemberUpdateDto newMember, ClaimsPrincipal claimedUser);

        Task<PhotoDto> AddPhoto(IFormFile file, ClaimsPrincipal claimedUser);

        Task<int> SetMainPhoto(int photoId, ClaimsPrincipal claimedUser);

        Task<int> DeletePhoto(int photoId, ClaimsPrincipal claimedUser);
    }
}
