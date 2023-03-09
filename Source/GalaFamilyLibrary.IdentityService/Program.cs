using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;
using GalaFamilyLibrary.IdentityService.Services;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.IdentityService.Models;
using AutoMapper;
using GalaFamilyLibrary.IdentityService.Helpers;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddScoped(typeof(IUserService), typeof(UserService));
builder.AddGenericSetup();

services.AddSingleton(provider => new MapperConfiguration(config =>
{
    var profile = new MappingProfiles(provider.GetService<IAESEncryptionService>());
    config.AddProfile(profile);
}).CreateMapper());
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseInitSeed(dbSeed =>
{
    dbSeed.InitTables(typeof(Program));
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }

    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");
    var file = string.Format(seedFolder, "Users");
    dbSeed.InitSeed<LibraryUser>(file);

    file = string.Format(seedFolder, "Roles");
    dbSeed.InitSeed<LibraryRole>(file);

    file = string.Format(seedFolder, "UserRoles");
    dbSeed.InitSeed<UserRole>(file);
});
app.UseGeneric();