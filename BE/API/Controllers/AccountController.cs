using Business.Service.Accounts;
using Common.Exceptions;
using DataLogic.Mappings.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            try
            {
                return await _accountService.Login(loginDto);
            }
            catch (ApiException ex)
            {
                switch (ex.StatusCode)
                {
                    case 401:
                        return Unauthorized(ex.Message);
                    case 400:
                        return BadRequest(ex.Message);
                    default:
                        return BadRequest("Problem in returning the data");
                }
            }


        }
    }
}
