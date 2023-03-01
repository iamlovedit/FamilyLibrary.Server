﻿using GalaFamilyLibrary.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SqlSugar.Extensions;
using System.Security.Claims;
using System.Text;

namespace GalaFamilyLibrary.Infrastructure.ServiceExtensions
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
        
            var audienceSection = configuration.GetSection("Audience");
            var key = audienceSection["Key"];
            var keyByteArray = Encoding.ASCII.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var issuer = audienceSection["Issuer"];
            var audience = audienceSection["Audience"];
            var expiration = audienceSection["Expiration"];

            services.AddSingleton(new PermissionRequirement(ClaimTypes.Role, issuer, audience,
                TimeSpan.FromSeconds(expiration.ObjToInt()), signingCredentials));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ElevatedRights",
                    policy => policy.RequireRole("Administrator", "Consumer").Build());
            });
        }
    }
}