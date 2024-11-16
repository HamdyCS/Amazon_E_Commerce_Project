using DataAccessLayer.Data;
using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Extensions;
using ApiLayer.Extensions;
using BusinessLayer.Options;
using BusinessLayer.Mapper.Profiles;
using DataAccessLayer.SoftDelete;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>
    (o => o.UseSqlServer(builder.Configuration.GetConnectionString("sqlServerConnectionString"))/*.AddInterceptors(new SoftDeleteInterceptor())*/);
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAutoMapper(typeof(PersonProfile));


var jwtOptions = builder.Configuration.GetSection("jwt").Get<JwtOptions>();


if (jwtOptions != null)
{
    builder.Services.AddSingleton(jwtOptions);
}
else
{
    Environment.Exit(0);
}

var mailOptions = builder.Configuration.GetSection("Mail").Get<MailOptions>();

if (mailOptions != null)
{
    builder.Services.AddSingleton(mailOptions);
}
else
{ 
    Environment.Exit(0); 
}


builder.Services.AddCustomJwtBearer(jwtOptions);

builder.UseSerilog();

builder.Services.AddCustomRepositoriesFromDataAccessLayer().AddCustomServiceseFromBusinessLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddCustomMiddlewares();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
