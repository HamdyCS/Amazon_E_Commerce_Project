using BusinessLayer.Contracks;
using BusinessLayer.Mapper;
using BusinessLayer.Mapper.Contracks;
using BusinessLayer.Servicese;
using DataAccessLayer.Contracks;
using DataAccessLayer.Repositories;
using DataAccessLayer.UnitOfWork;
using DataAccessLayer.UnitOfWork.Contracks;

namespace ApiLayer.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServiceseFromBusinessLayer(this IServiceCollection services)
        {

            services.AddScoped<IGenericMapper, GenericMapper>();


            services.AddScoped<IPersonService, PersonService>();

            services.AddScoped<IPersonServices, PersonServicese>();

            return services;
           
        }

        public static IServiceCollection AddCustomRepositoriesFromDataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped<ICityRepository,CityRepository>();

            services.AddScoped<IPersonRepository, PersonRepository>();

            services.AddScoped<IPhoneRepository, PhoneRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IRoleManager, RoleManager>();

            services.AddScoped<IUserAdderssRepository,UserAddressRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

           
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        
    }
}
