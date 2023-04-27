using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataLogic.Data;
using DataLogic.Entities;
using DataLogic.Mappings.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public Task<int> CreateUser(string email, string heroName)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<MemberDto> GetMemberAsync(string email)
        {
            return await _context.Users.Where(user => user.Email == email)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)?.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUserAsync()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public void UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email == email);
        }

        public async Task<User> AddUserAsync(string email, string heroNmae)
        {
            using var hmac = new HMACSHA512();
            var user = new User
            {
                HeroName = heroNmae,
                FirstName = heroNmae,
                LastName = heroNmae,
                Email = email,
                Created = DateTime.UtcNow,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$word")),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            if (await SaveAllAsync()) return user;
            throw new Exception();

        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(user => user.Email == email);
        }

        public async void ChangePassword(User user, string oldPassword, string newPassword)
        {
            if (oldPassword == newPassword) return;
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(oldPassword));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) throw new UnauthorizedAccessException();
            }
            using var newHmac = new HMACSHA512();
            user.PasswordHash = newHmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            user.PasswordSalt = newHmac.Key;
            user.LastActive = DateTime.UtcNow;
            _context.Entry(user).State = EntityState.Modified;

        }
    }
}
