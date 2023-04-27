using AutoMapper;
using Business.Service.Email;
using Business.Service.Users;
using Common.Exceptions;
using Common.Extensions;
using DataLogic.Mappings.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UsersController(IMapper mapper, IUserService userService, IEmailService emailService)
        {
            _mapper = mapper;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            Console.WriteLine(User.GetEmail());
            var user = await _userService.GetUsers();
            return Ok(user);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<MemberDto>> GetUser(string email)
        {
            return await _userService.GetUser(email);
        }

        [HttpPost("add-user")]
        public async Task<ActionResult> AddNewUser(NewMemberDto newMemberDto)
        {
            try
            {
                int status = await _userService.AddUser(newMemberDto);
                string url = $"https://localhost/api/users/{newMemberDto.Email}";
                _emailService.SendEmail(newMemberDto.Email, "New invitation",
                    "<p>Hello \n You have been invited to our private site.</p>" +
                    "<p>To log in, use this super secret password:'Pa$$word'</p>" +
                    "<p>See you<a href= 'http://localhost:4200' > here </ a ></p> ");
                return Ok(status);
            }
            catch (ApiException ex)
            {
                switch (ex.StatusCode)
                {
                    case 400:
                        return BadRequest(ex.Message);
                    default:
                        return BadRequest();
                }
            }
        }

        [HttpPut("change-password")]
        public async Task<ActionResult> UpdatePassword(PasswordUpdateDto passwordUpdate)
        {
            try
            {
                int status = await _userService.UpdatePassword(passwordUpdate, User);
                return Ok();
            }
            catch (ApiException ex)
            {
                switch (ex.StatusCode)
                {
                    case 400:
                        return BadRequest(ex.Message);
                    case 404:
                        return NotFound();
                    default:
                        return BadRequest("Failed changing the password");
                }
            }
        }

        // TODO
        // UpdateUser
        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto updates)
        {
            try
            {
                if (await _userService.UpdateUser(updates, User)) return Ok();
                return BadRequest("Problem updating user");
            }
            catch (ApiException ex)
            {
                switch (ex.StatusCode)
                {
                    case 400:
                        return BadRequest(ex?.Message);
                    case 404:
                        return NotFound(ex?.Message);
                    default:
                        return BadRequest("Problem updating user");
                }
            }
        }

        // AddPhoto
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            try
            {
                return CreatedAtAction(nameof(GetUser), new { email = User.GetEmail() }, await _userService.AddPhoto(file, User));
            }
            catch (ApiException ex)
            {
                switch (ex.StatusCode)
                {
                    case 400:
                        return BadRequest(ex?.Message);
                    case 404:
                        return NotFound(ex?.Message);
                    default:
                        return BadRequest("Problem adding photo");
                }
            }
        }

        // SetMainPhoto
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            try
            {
                int statusCode = await _userService.SetMainPhoto(photoId, User);
                return NoContent();
            }
            catch (ApiException ex)
            {
                switch (ex.StatusCode)
                {
                    case 400:
                        return BadRequest(ex?.Message);
                    case 404:
                        return NotFound(ex?.Message);
                    default:
                        return BadRequest("Problem setting the main photo");
                }
            }
        }

        // DeletePhoto
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            try
            {
                int statusCode = await _userService.DeletePhoto(photoId, User);
                return Ok();
            }
            catch (ApiException ex)
            {
                switch (ex.StatusCode)
                {
                    case 400:
                        return BadRequest(ex?.Message);
                    case 404:
                        return NotFound(ex?.Message);
                    default:
                        return BadRequest();
                }
            }
        }
    }
}
