using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;

namespace ProductManager.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommandHandler(
    ILogger<UnassignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<UnassignUserRoleCommand>
{
    public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Unassigning user role: {@Request}", request);

        var user = await userManager.FindByEmailAsync(request.UserEmail);

        if (user == null)
            throw new NotFoundException(nameof(User), request.UserEmail);
        
        var role = await roleManager.FindByNameAsync(request.RoleName);
        
        if (role == null)
            throw new NotFoundException(nameof(IdentityRole), request.RoleName);
        
        await userManager.RemoveFromRoleAsync(user, request.RoleName);
    }
}
