using System;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using ProductManager.Application.Products.Commands.CreateProduct;
using ProductManager.Application.Products.Commands.EditProduct;
using ProductManager.Application.Products.Dtos;
using ProductManager.Domain.Entities;
using Xunit;

namespace ProductManager.Application.Tests.Products.Dtos;

[TestSubject(typeof(ProductsProfile))]
public class ProductsProfileTest
{
    private readonly IMapper _mapper;

    public ProductsProfileTest()
    {
        var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<ProductsProfile>(); });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void CreateMap_FromProductToProductDto_MapsCorrectly()
    {
        var product = new Product
        { 
           Id = 1,
           Name = "Test",
           IsAvailable = true,
           ProduceDate = DateOnly.FromDateTime(DateTime.Now),
           ManufactureContact = new ManufactureContact
           {
               ManufactureEmail = "test@test.com",
               ManufacturePhone = "09876543211",
           }
        };

        var productDto = _mapper.Map<ProductDto>(product);

        productDto.Should().NotBeNull();
        productDto.ManufactureEmail.Should().Be(product.ManufactureContact.ManufactureEmail);
        productDto.ManufacturePhone.Should().Be(product.ManufactureContact.ManufacturePhone);
    }

    [Fact]
    public void CreateMap_FromCreateProductCommandToProduct_MapsCorrectly()
    {
        var command = new CreateProductCommand
        {
            Name = "Test",
            IsAvailable = true,
            ManufactureEmail = "test@test.com",
            ManufacturePhone = "09876543211",
            ProduceDate = DateOnly.FromDateTime(DateTime.Now)
        };
    
        var product = _mapper.Map<Product>(command);
    
        product.ManufactureContact.ManufactureEmail.Should().Be(command.ManufactureEmail);
        product.ManufactureContact.ManufacturePhone.Should().Be(command.ManufacturePhone);
        product.ProduceDate.Should().Be(command.ProduceDate);
    }
    
    [Fact]
    public void CreateMap_FromEditProductCommandToProduct_MapsCorrectly()
    {
        var command = new EditProductCommand
        {
            Name = "Test",
            IsAvailable = false
        };
    
        var product = _mapper.Map<Product>(command);
    
        product.Name.Should().Be(command.Name);
        product.IsAvailable.Should().Be(command.IsAvailable);
    }
}
