using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Exceptions;

namespace ProductManager.Application.Users.Commands.EditUserDetails;

public class EditUserDetailsCommandHandler(ILogger<EditUserDetailsCommandHandler> logger, UserManager<User> userManager, IUserContext userContext) : IRequestHandler<EditUserDetailsCommand>
{
    public async Task Handle(EditUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        
        logger.LogInformation("Updating user {User} with {RequestData}", user?.Id, request);
        
        var dbUser = await userManager.FindByIdAsync(user!.Id);

        if (dbUser == null)
            throw new NotFoundException(nameof(User), user.Id);


        dbUser.FirstName = request.FirstName;
        dbUser.LastName = request.LastName;
        await userManager.UpdateAsync(dbUser);        
    }
}
