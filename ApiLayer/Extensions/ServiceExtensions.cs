using BusinessLayer.Contracks;
using BusinessLayer.Mapper;
using BusinessLayer.Mapper.Contracks;
using BusinessLayer.Options;
using BusinessLayer.Servicese;
using DataAccessLayer.Contracks;
using DataAccessLayer.Repositories;
using DataAccessLayer.UnitOfWork;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BusinessLayer.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServiceseFromBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IGenericMapper, GenericMapper>();

            services.AddScoped<IPersonService, PersonService>();

            services.AddScoped<ICityService, CityService>();

            services.AddScoped<IRoleManagerRepository, RoleManagerRepository>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IMailService, MailService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IOtpService, OtpService>();

            services.AddScoped<IUserAddressService, UserAddressService>();

            services.AddScoped<IProductCategoryImageService, ProductCategoryImageService>();

            services.AddScoped<IProductCategoryService, ProductCategoryService>();

            services.AddScoped<IBrandService,BrandService>();

            services.AddScoped<IProductSubCategoryService, ProductSubCategoryService>();

            return services;

        }

        public static IServiceCollection AddCustomRepositoriesFromDataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped<ICityRepository, CityRepository>();

            services.AddScoped<IPersonRepository, PersonRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IRoleManagerRepository, RoleManagerRepository>();

            services.AddScoped<IUserAdderssRepository, UserAddressRepository>();

            services.AddScoped<IOtpRepository, OtpRepository>();

            services.AddScoped<IProductCategoryImageRepository, ProductCategoryImageRepository>();

            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

            services.AddScoped<IBrandRepository, BrandRepository>();

            services.AddScoped<IProductSubCategoryRepository, ProductSubCategoryRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddCustomJwtBearer(this IServiceCollection services, JwtOptions jwtOptions)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;// وهو المخطط المستخدم للتحقق من المستخدم.
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//وهو المخطط المستخدم لتحدي المستخدم عند عدم المصادقة
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuar,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateLifetime = true,


                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),

                        ClockSkew = TimeSpan.Zero,

                        TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.EncryptionKey.Substring(0, 16))),
                    };


                });

           
            return services;
        }

    }

}
