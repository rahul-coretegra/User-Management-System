using Microsoft.EntityFrameworkCore;
using User_Management_System.ManagementConfigurations;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using User_Management_System;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using User_Management_System.AuthorizationResources;
using Microsoft.AspNetCore.Identity.UI.Services;

using User_Management_System.ManagementRepository.IManagementRepository;
using User_Management_System.ManagementRepository;
using User_Management_System.PostgreSqlConfigurations;
using User_Management_System.MicrosoftSqlServerConfigurations;
using User_Management_System.MongoDbConfigurations;
using User_Management_System.PostgreSqlRepository.IPsqlRepository;
using User_Management_System.PostgreSqlRepository;
using User_Management_System.PostgreSqlRepository.RegisterAndAuthenticate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<PostgreSqlApplicationDbContext>(options =>
     options.UseNpgsql(builder.Configuration.GetSection("PostgreSqlConfigurations")["PostgreSqlConnectionString"]));

builder.Services.AddDbContext<MicrosoftSqlServerApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSection("MicrosoftSqlServerConfigurations")["MicrosoftSqlServerConnection"]));

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbConfigurations"));



// Register MongoDBContext as a singleton
builder.Services.AddSingleton<MongoDbApplicationDbContext>();


builder.Services.AddScoped<IDbContextConfigurations, DbContextConfigurations>();
builder.Services.AddScoped<IManagementWork, ManagementWork>();

builder.Services.AddScoped<IPsqlUnitOfWork, PsqlUnitOfWork>();
builder.Services.AddScoped<IRegisterAndAuthenticateRepository,RegisterAndAuthenticationRepository>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings);
var appSetting = appSettings.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSetting.Secret);

builder.Services.ConfigureJwtAuthentication(key);
builder.Services.ConfigureCustomAuthorization();

builder.Services.AddScoped<IAuthorizationHandler, SupremeAccessAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
