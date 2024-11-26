using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProductManager.Domain.Constants;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;

namespace ProductManager.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    ILogger<RegisterUserCommandHandler> logger,
    UserManager<User> userManager,
    IMapper mapper)
    : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering user {User}", request);
        
        var existingUser = await userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new ConflictException("User already exists with this email.");
        }
        
        var user = mapper.Map<User>(request);
        
        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var roleResult = await userManager.AddToRoleAsync(user, UserRoles.User);

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                logger.LogError("User creation failed: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                throw new Exception("User created but failed to assign role.");
            }
            
        }
        else
        {
            logger.LogError("User creation failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            throw new Exception("User creation failed.");
        }
    }
}
