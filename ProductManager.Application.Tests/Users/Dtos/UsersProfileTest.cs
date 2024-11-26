using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using ProductManager.Application.Users.Commands.RegisterUser;
using ProductManager.Application.Users.Dtos;
using ProductManager.Domain.Entities;
using Xunit;

namespace ProductManager.Application.Tests.Users.Dtos;

[TestSubject(typeof(UsersProfile))]
public class UsersProfileTest
{
    private readonly IMapper _mapper;

    private const string Email = "test@test.com";

    public UsersProfileTest()
    {
        var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<UsersProfile>(); });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void CreateMap_FromUserToUserDto_MapsCorrectly()
    {
        var user = new User()
        {
            Id = "1",
            Email = Email,
            FirstName = "Test",
            LastName = "Test",
            NormalizedEmail = Email.ToUpper(),
            Products = []
        };

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Should().NotBeNull();
        userDto.Email.Should().Be(user.Email);
    }

    [Fact]
    public void CreateMap_FromRegisterUserCommandToUser_MapsCorrectly()
    {
        var command = new RegisterUserCommand()
        {
            Email = Email,
            Password = "password",
            FirstName = "Test",
            LastName = "Test",
        };

        var user = _mapper.Map<User>(command);

        user.Email.Should().Be(command.Email);
        user.NormalizedEmail.Should().Be(command.Email.ToUpper());
        user.UserName.Should().Be(command.Email);
        user.NormalizedUserName.Should().Be(command.Email.ToUpper());
    }
}
