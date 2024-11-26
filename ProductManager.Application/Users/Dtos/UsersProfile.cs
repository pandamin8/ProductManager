using AutoMapper;
using ProductManager.Application.Users.Commands.RegisterUser;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Users.Dtos;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<RegisterUserCommand, User>()
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpperInvariant()))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.ToUpperInvariant()));
    }
}
