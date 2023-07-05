using AutoMapper;
using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.IdentityService.Helpers;
using GalaFamilyLibrary.IdentityService.Services;
using GalaFamilyLibrary.Infrastructure.Middlewares;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;
using GalaFamilyLibrary.Infrastructure.Seed;
using GalaFamilyLibrary.Infrastructure.ServiceExtensions;

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
    dbSeed.InitTablesByClass(typeof(ApplicationUser));
    var wwwRootDirectory = app.Environment.WebRootPath;
    if (string.IsNullOrEmpty(wwwRootDirectory))
    {
        return;
    }

    var seedFolder = Path.Combine(wwwRootDirectory, "Seed/{0}.json");
    var file = string.Format(seedFolder, "Users");
    dbSeed.InitSeed<ApplicationUser>(file);

    file = string.Format(seedFolder, "Roles");
    dbSeed.InitSeed<ApplicationRole>(file);

    file = string.Format(seedFolder, "UserRoles");
    dbSeed.InitSeed<UserRole>(file);

    file = string.Format(seedFolder, "FamilyCollections");
    dbSeed.InitSeed<FamilyCollection>(file);

    file = string.Format(seedFolder, "FamilyStars");
    dbSeed.InitSeed<FamilyStar>(file);
});
app.UseGeneric();