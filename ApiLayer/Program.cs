using ApiLayer.Filters;
using BusinessLayer.BackgroundServices;
using BusinessLayer.Extensions;
using BusinessLayer.Mapper.Profiles;
using BusinessLayer.Options;
using DataAccessLayer.Data;
using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
//builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext>
    (o => o.UseSqlServer(builder.Configuration.GetConnectionString("sqlServerConnectionString"))/*.AddInterceptors(new SoftDeleteInterceptor())*/);
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAutoMapper(typeof(PersonProfile));

//jwt options
var jwtOptions = builder.Configuration.GetSection("jwt").Get<JwtOptions>();


if (jwtOptions != null)
{
    builder.Services.AddSingleton(jwtOptions);
}
else
{
    Environment.Exit(0);
}

//mail options
var mailOptions = builder.Configuration.GetSection("Mail").Get<MailOptions>();

if (mailOptions != null)
{
    builder.Services.AddSingleton(mailOptions);
}
else
{
    Environment.Exit(Environment.ExitCode);
}



builder.UseSerilog();

//stripe key
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

//stripe options 
var stripeOptions = builder.Configuration.GetSection("Stripe").Get<StripeOptions>();
if (stripeOptions != null)
{
    builder.Services.AddSingleton(stripeOptions);
}
else
{
    Environment.Exit(Environment.ExitCode);
}

//Cloudinary
var cloudinaryOptions = builder.Configuration.GetSection("Cloudinary").Get<CloudinaryOptions>();
if (cloudinaryOptions != null)
{
    builder.Services.AddSingleton(cloudinaryOptions);
}
else
{
    Environment.Exit(Environment.ExitCode);
}

//redis
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration["Redis:ConnectionString"];
    opt.InstanceName = builder.Configuration["Redis:InstanceName"];
});



builder.Services.AddCustomRepositoriesFromDataAccessLayer().AddCustomServiceseFromBusinessLayer();
builder.Services.AddCustomJwtBearer(jwtOptions);

var rateLimitOptions = builder.Configuration.GetSection("RateLimitOption").Get<RateLimitOptions>();
if (rateLimitOptions != null)
{
    builder.Services.AddCustomRateLimiting(rateLimitOptions);
}
else
{
    Environment.Exit(Environment.ExitCode);
}

//Authentication by providers

//github auth
builder.Services.AddAuthentication().AddCookie().AddGitHub("GitHub", githubOptions =>
{
    githubOptions.ClientId = builder.Configuration["Authentication:Github:ClientId"]!;
    githubOptions.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];
});

//google auth
builder.Services.AddAuthentication().AddGoogle("Google", googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


//add filters
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<CheckIfUserIsNotDeletedFilter>();
});

//add background services
builder.Services.AddHostedService<ProductsCacheUpdateBackgroundService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // app.MapOpenApi();

}



app.UseHttpsRedirection();
app.UseAuthentication(); // إضافة المصادقة أولاً
app.UseAuthorization();  // ثم التفويض
app.UseRateLimiter();// عشان ال rate limter
app.MapControllers();
//app.AddCustomMiddlewares();

app.Run();





