using DataLogic.Entities;
using DataLogic.Mappings.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Repositories.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUserAsync();

        Task<User> GetUserByIdAsync(int id);

        Task<User> GetUserByEmailAsync(string email);

        Task<IEnumerable<MemberDto>> GetMembersAsync();

        Task<MemberDto> GetMemberAsync(string email);

        //Task<int> CreateUser(string email, string heroName);

        void UpdateUser(User user);

        Task<bool> SaveAllAsync();

        Task<bool> UserExists(string email);

        Task<User> AddUserAsync(string email, string heroName);

        void ChangePassword(User user, string oldPassword, string newPassword);
    }
}
