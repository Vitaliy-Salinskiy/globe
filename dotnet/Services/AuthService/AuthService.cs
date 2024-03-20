using AutoMapper;
using dotnet.Dtos;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AuthService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserModel>> Register(CreateUserDto userDto)
        {
            var serviceResponse = new ServiceResponse<UserModel>();
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                var newUser = await _context.Users.AddAsync(new UserModel
                {
                    Username = userDto.Username,
                    HashedPassword = hashedPassword,
                    Email = userDto.Email,
                    Address = userDto.Address,
                    DataOfBirth = userDto.DataOfBirth
                });

                await _context.SaveChangesAsync();

                if (newUser != null)
                {
                    serviceResponse.Message = "User has been created successfully";
                    serviceResponse.Data = newUser.Entity;
                    return serviceResponse;
                }


                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Something went wrong, please try again later.";
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<UserModel>> Login(LoginDto loginDto)
        {
            var serviceResponse = new ServiceResponse<UserModel>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(loginDto.Email.ToLower()));

                if (user == null)
                {
                    serviceResponse.Data = null;
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"User not with email: {loginDto.Email} found.";
                    return serviceResponse;
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword);

                if (!isPasswordValid)
                {
                    serviceResponse.Data = null;
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Invalid password.";
                    return serviceResponse;
                }

                serviceResponse.Data = user;
                serviceResponse.Message = "User has been logged in successfully.";
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

    }
}