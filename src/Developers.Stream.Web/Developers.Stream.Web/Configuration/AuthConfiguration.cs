using Developers.Stream.Infrastructure.Auth;
using Developers.Stream.Infrastructure.Auth.Contexts;
using Developers.Stream.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Developers.Stream.Web.Configuration;

public static class AuthConfiguration
{
    public static IHostApplicationBuilder AddAuth(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddTwitch(configuration =>
            {
                configuration.ClientId = builder.Configuration["twitch:clientId"]!;
                configuration.ClientSecret = builder.Configuration["twitch:secret"]!;
            })
            .AddIdentityCookies();

        builder.Services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("developers-stream-auth"));
        });

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        return builder;
    }
}