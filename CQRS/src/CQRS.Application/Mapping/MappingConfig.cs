using CQRS.Application.DTOs;
using CQRS.Domain.Entities;
using Mapster;

namespace CQRS.Application.Mapping;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Bank, BankDto>.NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Bic, src => src.Bic)
            .Map(dest => dest.Country, src => src.Country);

        TypeAdapterConfig<BankDto, Bank>.NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Bic, src => src.Bic)
            .Map(dest => dest.Country, src => src.Country);
    }
}