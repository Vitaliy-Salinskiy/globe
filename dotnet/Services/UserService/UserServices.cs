
using Microsoft.EntityFrameworkCore;

namespace dotnet.Services.UserService
{
    public class UserService : IUserService
    {

        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<UserModel>> CheckEmail(string email)
        {
            var serviceResponse = new ServiceResponse<UserModel>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));

                if (user == null)
                {
                    serviceResponse.Data = null;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Email is available.";

                }
                else
                {
                    serviceResponse.Data = user;
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Email is already taken.";
                }

                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }
    }
}