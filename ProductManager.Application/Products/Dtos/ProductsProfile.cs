using AutoMapper;
using ProductManager.Application.Products.Commands.CreateProduct;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Products.Dtos;

public class ProductsProfile : Profile
{
    public ProductsProfile()
    {
        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.ManufactureContact, opt => opt.MapFrom(
                command => new ManufactureContact
                {
                    ManufactureEmail = command.ManufactureEmail,
                    ManufacturePhone = command.ManufacturePhone
                }
            ));
    }
}
