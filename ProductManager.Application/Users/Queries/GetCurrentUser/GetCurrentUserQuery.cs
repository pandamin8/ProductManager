using MediatR;
using ProductManager.Application.Users.Dtos;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Users.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<UserDto>
{
    
}
