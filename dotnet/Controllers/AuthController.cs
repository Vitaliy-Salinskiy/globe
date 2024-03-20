using dotnet.Services.AuthService;
using dotnet.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using dotnet.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using dotnet.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace dotnet.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        public AuthController(IAuthService authService, IUserService userService, IOptions<AppSettings> appSettings)
        {
            _authService = authService;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<UserModel>>> Register([FromBody] CreateUserDto userDto)
        {
            var candidate = await _userService.CheckEmail(userDto.Email);

            if (candidate.Data != null)
            {
                return BadRequest(candidate);
            };


            var isCreated = await _authService.Register(userDto);

            if (isCreated.Success)
            {
                return Ok(isCreated);
            }

            return BadRequest(isCreated);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login([FromBody] LoginDto loginDto)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var loginData = await _authService.Login(loginDto);

                if (loginData.Success && loginData.Data != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();

                    var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity([
                            new Claim("sub", loginData.Data.Id.ToString()),
                        new Claim(ClaimTypes.Email, loginData.Data.Email),
                        new Claim("name", loginData.Data.Username)
                        ]),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    serviceResponse.Data = tokenString;
                    serviceResponse.Message = "Login successful";
                    return Ok(serviceResponse);
                }

                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid email or password";
                return Unauthorized(serviceResponse);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, serviceResponse);
            }
        }
    }
}