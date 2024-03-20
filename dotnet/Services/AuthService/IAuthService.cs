using dotnet.Dtos;

namespace dotnet.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<UserModel>> Register(CreateUserDto userDto);

        Task<ServiceResponse<UserModel>> Login(LoginDto loginDto);
    }
}