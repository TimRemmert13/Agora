using API.Data;
using API.Data.Respositories;
using API.Interfaces;
using API.Services;
using API.Utilities;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite("Data Source=agora.db");
            });
            services.AddSingleton(x => new BlobServiceClient(config.GetConnectionString("AzureStorage")));
            services.AddSingleton<IAzureStorageService, AzureStorageService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IArtWorkRepository, ArtWorkRespository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddControllers().AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            return services;
        }

    }
}