using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ServiceStack;
using ServiceStack.Auth;
using MyApp.Data;
using ServiceStack.Configuration;
using ServiceStack.Text;

[assembly: HostingStartup(typeof(MyApp.ConfigureAuth))]

namespace MyApp;

public class ConfigureAuth : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            services.AddSingleton<IAuthHttpGateway, AuthHttpGateway>();
            services.AddTransient<IExternalLoginAuthInfoProvider,ExternalLoginAuthInfoProvider>();
        })
        .ConfigureAppHost(appHost => 
        {
            appHost.Plugins.Add(new AuthFeature(IdentityAuth.For<ApplicationUser>(options => {
                options.EnableCredentialsAuth = true;
                options.SessionFactory = () => new CustomUserSession();
            })));
        });
}

public interface IExternalLoginAuthInfoProvider
{
    void PopulateUser(ExternalLoginInfo info, ApplicationUser user);
}

// Populate ApplicationUser with Auth Info
public class ExternalLoginAuthInfoProvider(IConfiguration configuration, IAuthHttpGateway authGateway)
    : IExternalLoginAuthInfoProvider
{
    public void PopulateUser(ExternalLoginInfo info, ApplicationUser user)
    {
        var accessToken = info.AuthenticationTokens?.FirstOrDefault(x => x.Name == "access_token");
        //var accessTokenSecret = info.AuthenticationTokens?.FirstOrDefault(x => x.Name == "access_token_secret");

        if (info.LoginProvider == "Facebook")
        {
            user.FacebookUserId = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            user.DisplayName = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            user.FirstName = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            user.LastName = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;

            if (accessToken != null)
            {
                var facebookInfo = JsonObject.Parse(authGateway.DownloadFacebookUserInfo(accessToken.Value, "picture"));
                var picture = facebookInfo.Object("picture");
                var data = picture?.Object("data");
                if (data != null)
                {
                    if (data.TryGetValue("url", out var profileUrl))
                    {
                        user.ProfileUrl = profileUrl.SanitizeOAuthUrl();
                    }
                }                
            }
        }
        else if (info.LoginProvider == "Google")
        {
            user.GoogleUserId = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            user.DisplayName = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            user.FirstName = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            user.LastName = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            user.GoogleProfilePageUrl = info.Principal?.Claims.FirstOrDefault(x => x.Type == "urn:google:profile")?.Value;

            if (accessToken != null)
            {
                var googleInfo = JsonObject.Parse(authGateway.DownloadGoogleUserInfo(accessToken.Value));
                user.ProfileUrl = googleInfo.Get("picture").SanitizeOAuthUrl();
            }
        }
        else if (info.LoginProvider == "Microsoft")
        {
            user.MicrosoftUserId = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            user.DisplayName = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            user.FirstName = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            user.LastName = info.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            if (accessToken != null)
            {
                user.ProfileUrl = authGateway.CreateMicrosoftPhotoUrl(accessToken.Value, "96x96");
            }
        }
    }
}

public static class ClaimUtils
{
    public static string PermissionType { get; set; } = "perm";
    public static string Picture { get; set; } = "picture";

    public static bool IsAuthenticated(this ClaimsPrincipal? principal) => principal?.Identity?.IsAuthenticated == true;
    public static ClaimsPrincipal? AuthenticatedUser(this ClaimsPrincipal? principal) =>
        principal?.Identity?.IsAuthenticated == true ? principal : null;
    public static bool IsAdmin(this ClaimsPrincipal? principal) => principal?.GetRoles().Contains(RoleNames.Admin) != null;

    public static string? GetUserId(this ClaimsPrincipal? principal) => principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public static string? GetDisplayName(this ClaimsPrincipal? principal) => principal?.FindFirst(ClaimTypes.Name)?.Value;
    public static string? GetEmail(this ClaimsPrincipal principal) => principal.FindFirst(ClaimTypes.Email)?.Value;
    public static string[] GetRoles(this ClaimsPrincipal? principal) => principal?.Claims.Where(x => x.Type == ClaimTypes.Role)
        .Select(x => x.Value).ToArray() ?? Array.Empty<string>();
    public static string[] GetPermissions(this ClaimsPrincipal? principal) => principal?.Claims.Where(x => x.Type == PermissionType)
        .Select(x => x.Value).ToArray() ?? Array.Empty<string>();

    public static bool HasRole(this ClaimsPrincipal? principal, string roleName) => principal?.GetRoles()
        .Contains(roleName) == true;
    public static bool HasAllRoles(this ClaimsPrincipal? principal, params string[] roleNames) => principal?.GetRoles()
        .All(roleNames.Contains) == true;

    public static string? GetProfileUrl(this ClaimsPrincipal? principal) => 
        X.Map(principal?.FindFirst(Picture)?.Value, x => string.IsNullOrWhiteSpace(x) ? null : x)
        ?? JwtClaimTypes.DefaultProfileUrl;
}
