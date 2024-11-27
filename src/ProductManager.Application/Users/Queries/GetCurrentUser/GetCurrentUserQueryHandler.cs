using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProductManager.Application.Users.Dtos;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;

namespace ProductManager.Application.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(
    ILogger<GetCurrentUserQueryHandler> logger,
    UserManager<User> userManager,
    IUserContext userContext,
    IMapper mapper) : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var authUser = userContext.GetCurrentUser();

        if (authUser == null)
            throw new ForbidException();
        
        logger.LogInformation("User {UserId} is retrieving its data", authUser.Id);

        var user = await userManager.FindByIdAsync(authUser.Id);

        if (user == null)
            throw new ForbidException();
        
        var roles = await userManager.GetRolesAsync(user);
        
        var userDto = mapper.Map<UserDto>(user);
        
        userDto.Roles = roles.ToList();
        
        return userDto;
    }
}
