using AutoMapper;
using Azure.Core;
using Business.Service.Photo;
using CloudinaryDotNet;
using Common.Exceptions;
using Common.Extensions;
using DataLogic.Entities;
using DataLogic.Mappings.DTOs;
using DataLogic.Repositories.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UserService(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<IEnumerable<MemberDto>> GetUsers()
        {
            return await _userRepository.GetMembersAsync();
        }

        public async Task<MemberDto> GetUser(string email)
        {
            return await _userRepository.GetMemberAsync(email);
        }

        public async Task<int> AddUser(NewMemberDto newMember)
        {



            if (await _userRepository.UserExists(newMember.Email))
            {
                throw new ApiException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Email is already taken"
                };
            }

            var user = _userRepository.AddUserAsync(newMember.Email, newMember.HeroName);
            //TODO

            if (user != null) return StatusCodes.Status201Created;

            throw new ApiException
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Failed to add new user"
            };
        }

        public async Task<int> UpdatePassword(PasswordUpdateDto passwordUpdate, ClaimsPrincipal claimedUser)
        {
            var user = await _userRepository.GetUserByEmailAsync(claimedUser.GetEmail());

            if (user == null) throw new ApiException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "There is no user woth this email"
            };

            try
            {
                _userRepository.ChangePassword(user, passwordUpdate.Password, passwordUpdate.NewPassword);
                if (await _userRepository.SaveAllAsync()) return StatusCodes.Status200OK;
                return StatusCodes.Status400BadRequest;
            }
            catch (Exception ex)
            {
                throw new ApiException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<bool> UpdateUser(MemberUpdateDto updates, ClaimsPrincipal claimedUser)
        {
            var user = await _userRepository.GetUserByEmailAsync(claimedUser.GetEmail());

            if (user == null) throw new ApiException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Entity not fount for the user: {user}"
            };

            _mapper.Map(updates, user);
            user.LastActive = DateTime.UtcNow;

            if (await _userRepository.SaveAllAsync()) return true;

            throw new ApiException
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Failed to update user"
            };
            //call user Repository
        }

        public async Task<PhotoDto> AddPhoto(IFormFile file, ClaimsPrincipal claimedUser)
        {
            var user = await _userRepository.GetUserByEmailAsync(claimedUser.GetEmail());

            if (user == null) throw new ApiException
            {
                StatusCode = StatusCodes.Status404NotFound,
            };

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                throw new ApiException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Error.Message,
                };
            }

            var photo = new DataLogic.Entities.Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            user.LastActive = DateTime.UtcNow;

            if (await _userRepository.SaveAllAsync()) return _mapper.Map<PhotoDto>(photo);

            throw new ApiException
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Problem adding photo"
            };
        }

        public async Task<int> SetMainPhoto(int photoId, ClaimsPrincipal claimedUser)
        {
            var user = await _userRepository.GetUserByEmailAsync(claimedUser.GetEmail());

            if (user == null) throw new ApiException
            {
                StatusCode = StatusCodes.Status404NotFound
            };

            //Repo task
            var photo = user.Photos.FirstOrDefault(photo => photo.Id == photoId);

            if (photo == null) throw new ApiException
            {
                StatusCode = StatusCodes.Status404NotFound
            };

            if (photo.IsMain) throw new ApiException
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "This is already your main photo"
            };

            //Repo task
            var currentMainPhoto = user.Photos.FirstOrDefault(photo => photo.IsMain);

            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
            }

            //Repo Task
            photo.IsMain = true;
            user.LastActive = DateTime.UtcNow;

            if (await _userRepository.SaveAllAsync())
                return StatusCodes.Status204NoContent;

            throw new ApiException
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Problem setting the main photo"
            };
        }

        public async Task<int> DeletePhoto(int photoId, ClaimsPrincipal claimedUser)
        {
            var user = await _userRepository.GetUserByEmailAsync(claimedUser.GetEmail());

            if (user == null)
                throw new ApiException
                {
                    StatusCode = StatusCodes.Status404NotFound
                };

            var photo = user.Photos.FirstOrDefault(photo => photoId == photo.Id);

            if (photo == null)
                throw new ApiException
                {
                    StatusCode = StatusCodes.Status404NotFound,
                };

            if (photo.IsMain)
                throw new ApiException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "You cannot delete your main photo"
                };

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Error != null)
                    throw new ApiException
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = result.Error.Message
                    };
            }

            //repo task
            user.Photos.Remove(photo);
            user.LastActive = DateTime.UtcNow;

            if (await _userRepository.SaveAllAsync()) return StatusCodes.Status200OK;

            throw new ApiException
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Problem deleting photo"
            };

        }
    }
}
