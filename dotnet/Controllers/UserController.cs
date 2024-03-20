using dotnet.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<UserModel>>> FindUserByEmail(string email)
        {
            return Ok(await _userService.CheckEmail(email));
        }
    }
}