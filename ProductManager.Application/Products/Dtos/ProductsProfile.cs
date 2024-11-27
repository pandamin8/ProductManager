using AutoMapper;
using ProductManager.Application.Products.Commands.CreateProduct;
using ProductManager.Application.Products.Commands.EditProduct;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Products.Dtos;

public class ProductsProfile : Profile
{
    public ProductsProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.ManufactureEmail, opt => opt.MapFrom(src => src.ManufactureContact.ManufactureEmail))
            .ForMember(d => d.ManufacturePhone, opt => opt.MapFrom(src => src.ManufactureContact.ManufacturePhone));

        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.ManufactureContact, opt => opt.MapFrom(
                command => new ManufactureContact
                {
                    ManufactureEmail = command.ManufactureEmail,
                    ManufacturePhone = command.ManufacturePhone
                }
            ));

        CreateMap<EditProductCommand, Product>();
    }
}
