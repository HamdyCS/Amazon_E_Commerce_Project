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

<<<<<<< HEAD
            services.AddScoped<IPersonService, PersonService>();
=======
            services.AddScoped<IPersonServices, PersonServicese>();
>>>>>>> cc8c5f0f0dc7b9ce001b93e674c52e553a9adc0b
            return services;
           
        }

        public static IServiceCollection AddCustomServiceseFromDataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
<<<<<<< HEAD
            services.AddScoped<IUserRepository, UserRepository>();
=======

>>>>>>> cc8c5f0f0dc7b9ce001b93e674c52e553a9adc0b



            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        
    }
}
