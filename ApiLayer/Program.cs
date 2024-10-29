using DataAccessLayer.Data;
using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Extensions;
using BusinessLayer.Mapper;
using BusinessLayer.Authentication;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("sqlServerConnectionString")));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
