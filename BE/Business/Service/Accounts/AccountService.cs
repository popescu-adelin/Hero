using Business.Service.Token;
using Common.Exceptions;
using DataLogic.Mappings.DTOs;
using DataLogic.Repositories.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public AccountService(IUserRepository userRepository, ITokenService token)
        {
            _userRepository = userRepository;
            _tokenService = token;
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null) throw new ApiException
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Invalid email"
            };
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) throw new ApiException
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Invalid password"
                };
            }

            return new UserDto
            {
                LastActive = user.LastActive,
                Email = user.Email,
                HeroName = user.HeroName,
                PhotoUrl = user?.Photos?.FirstOrDefault(photo => photo.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
            };

            throw new ApiException
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Problem in returning the data"
            };
        }
    }
}
