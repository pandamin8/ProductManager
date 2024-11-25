using AutoMapper;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Users.Dtos;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<User, UserDto>();
    }
}
