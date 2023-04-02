using Mapster;
using Microsoft.EntityFrameworkCore;
using RitRestAPI.Abstractions;
using RitRestAPI.Models.DTOs.DrillBlockDTOs;
using RitRestAPI.Models.DTOs.DrillBlockPointsDTOs;
using RitRestAPI.Models.DTOs.HoleDTOs;
using RitRestAPI.Models.DTOs.HolePointsDTOs;
using RitRestAPI.Services;

namespace RitRestAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped(typeof(IGenericRepositoryService<>), typeof(GenericRepositoryService<>))
            .AddMapsterConfiguration()
            .AddScoped<ISingleQueryService, SingleQueryService>();

    private static IServiceCollection AddMapsterConfiguration(this IServiceCollection services)
    { 
        TypeAdapterConfig<DrillBlockPointsDto, DrillBlockDto>.NewConfig()
            .Map(dest => dest.Id, src => src.DrillBlock);

        TypeAdapterConfig<HoleDto, DrillBlockDto>.NewConfig()
            .Map(dest => dest.Id, src => src.DrillBlock);

        TypeAdapterConfig<HolePointsDto, HoleDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Hole);

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<IRitRestDbContext, RitRestDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.EnableSensitiveDataLogging(false);
        });
}