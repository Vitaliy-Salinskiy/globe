using dotnet.Dtos;

namespace AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, CreateUserDto>();
        }
    }
}