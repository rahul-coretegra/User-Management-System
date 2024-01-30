using Microsoft.EntityFrameworkCore;
using User_Management_System.DbModule;
using User_Management_System.Repositories.IRepository.Repository;
using User_Management_System.Repositories.IRepository;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using User_Management_System;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using User_Management_System.AuthorizationResources;
using User_Management_System.Repositories.RegisterAndAuthenticate;
using User_Management_System.TwilioModule;
using Microsoft.AspNetCore.Identity.UI.Services;
using User_Management_System.OutlookSmtpConfigurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRegisterAndAuthenticateRepository, RegisterAndAuthenticationRepository>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IOutlookSmtpRepository, OutlookSmtpRepository>();

builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddScoped<ITwilioRepository, TwilioRepository>();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>,ConfigureSwaggerOptions>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var appSetting = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSetting.Secret);

builder.Services.ConfigureJwtAuthentication(key);
builder.Services.ConfigureCustomAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthorizationHandler, SupremeLevelAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, AuthorityLevelAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IntermediateLevelAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, SecondaryLevelAuthorizationHandler>();

builder.Services.AddScoped<IAuthorizationHandler, IsAccssAuthorizationHandler>();


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
