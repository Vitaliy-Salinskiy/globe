namespace dotnet.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<UserModel>> CheckEmail(string email);
    }
}