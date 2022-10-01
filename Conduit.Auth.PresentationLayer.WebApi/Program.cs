using System;
using System.Collections.Generic;
using System.Globalization;
using Conduit.Auth.ApplicationLayer.Users.GetCurrent;
using Conduit.Auth.ApplicationLayer.Users.Register;
using Conduit.Auth.DomainLayer.Services;
using Conduit.Auth.DomainLayer.Services.ApplicationLayer.Users;
using Conduit.Auth.DomainLayer.Users.Passwords;
using Conduit.Auth.DomainLayer.Users.Services;
using Conduit.Auth.InfrastructureLayer.JwtTokens;
using Conduit.Auth.InfrastructureLayer.MongoDB.DependencyInjection;
using Conduit.Auth.InfrastructureLayer.Services;
using Conduit.Auth.InfrastructureLayer.Users.Passwords;
using Conduit.Auth.InfrastructureLayer.Users.Services;
using Conduit.Auth.PresentationLayer.WebApi;
using Conduit.Shared.Events.Models.Users;
using Conduit.Shared.Events.Services.RabbitMQ;
using Conduit.Shared.Localization;
using Conduit.Shared.Startup;
using Conduit.Shared.Tokens;
using Conduit.Shared.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

#region ServicesConfiguration

var services = builder.Services;
var environment = builder.Environment;
var configuration = builder.Configuration;
var supportedCultures = new List<CultureInfo> { new("ru"), new("en") };
services.AddControllers().Localize<SharedResource>(supportedCultures);
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Conduit.Auth.PresentationLayer.WebApi", Version = "v1" });
});

services.AddHealthChecks().Services
    .AddMongoWithHealthChecks(configuration.GetSection("Mongo").Bind)
    .AddJwtIssuerServices().AddJwtServices(configuration.GetSection("Jwt").Bind)
    .AddW3CLogging(configuration.GetSection("W3C").Bind).AddHttpClient()
    .DisableDefaultModelValidation()
    .AddTransient<IPasswordManager, PasswordManager>()
    .AddSingleton<IIdManager, IdManager>()
    .AddSingleton<IImageChecker, ImageChecker>().AddHttpContextAccessor()
    .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
    .AddMediatR(typeof(GetCurrentUserRequestHandler).Assembly)
    .AddValidatorsFromAssembly(typeof(RegisterUserModelValidator).Assembly)
    .RegisterRabbitMqWithHealthCheck(configuration.GetSection("RabbitMQ").Bind)
    .RegisterProducer<RegisterUserEventModel>()
    .RegisterProducer<UpdateUserEventModel>();

#endregion

var app = builder.Build();

#region AppConfiguration

if (environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    IdentityModelEventSource.ShowPII = true;
}

app.UseRouting();
app.UseCors(options =>
    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseW3CLogging();
app.UseRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(x =>
{
    x.MapControllers();
    x.MapHealthChecks("/health");
});

var initializationScope = app.Services.CreateScope();

await initializationScope.WaitHealthyServicesAsync(TimeSpan.FromHours(1));
await initializationScope.InitializeMongoDbAsync();
await initializationScope.InitializeQueuesAsync();

#endregion

app.Run();
