using Microsoft.AspNetCore.Authorization;

namespace Valhalla_v3.Services;

public static class Policies
{
    public const string IsAdmin = "IsAdmin";
    public const string IsUserLog = "IsUserLog";
    public const string IsUser = "IsUser";
    public const string IsClaim = "IsClaim";

    public static AuthorizationPolicy IsAdminPolicy()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()

                                               .RequireRole("adminEdu")
                                               .Build();
    }

    public static AuthorizationPolicy IsUserLogged()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()

                                               .Build();
    }

    public static AuthorizationPolicy IsClaimed()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
            .RequireClaim("MyCos", "MyValue")
            .Build();
    }

    public static AuthorizationPolicy IsUserPolicy()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                               .RequireRole("User")
                                               .Build();
    }
}
