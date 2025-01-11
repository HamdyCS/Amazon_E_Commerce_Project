using BusinessLayer.Contracks;
using BusinessLayer.Mapper;
using BusinessLayer.Mapper.Contracks;
using BusinessLayer.Options;
using BusinessLayer.Servicese;
using DataAccessLayer.Contracks;
using DataAccessLayer.Entities;
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

            services.AddTransient<IMailService, MailService>();// لتجنب تعقيدات التزامن

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IOtpService, OtpService>();

            services.AddScoped<IUserAddressService, UserAddressService>();

            services.AddScoped<IProductCategoryImageService, ProductCategoryImageService>();

            services.AddScoped<IProductCategoryService, ProductCategoryService>();

            services.AddScoped<IBrandService,BrandService>();

            services.AddScoped<IProductSubCategoryService, ProductSubCategoryService>();

            services.AddScoped<IProductImageService, ProductImageService>();

            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<ISellerProductService,SellerProductService>();

            services.AddScoped<ISellerProductReviewService,SellerProductReviewService>();

            services.AddScoped<ICityWhereDeliveyWorkService, CityWhereDeliveyWorkService>();

            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            services.AddScoped<IProductInShoppingCartService, ProductInShoppingCartService>();
            
            services.AddScoped<IApplicationTypeService, ApplicationTypeService>();


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

            services.AddScoped<IProductImageRepository, ProductImageRepository>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ISellerProductRepository, SellerProductRepository>();

            services.AddScoped<ISellerProductReviewRepository, SellerProductReviewRepository>();

            services.AddScoped<ICityWhereDeliveyWorkRepository, CityWhereDeliveyWorkRepository>();

            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

            services.AddScoped<IProductsInShoppingCartRepository, ProductsInShoppingCartRepository>();

            services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>();

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
