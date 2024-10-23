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
            return services;
           
        }

        public static IServiceCollection AddCustomServiceseFromDataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();




            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        
    }
}
