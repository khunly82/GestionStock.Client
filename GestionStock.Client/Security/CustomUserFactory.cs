using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace GestionStock.Client.Security
{

    public class CustomUserFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public CustomUserFactory(IAccessTokenProviderAccessor accessor) : base(accessor) { }

        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);

            if (user.Identity?.IsAuthenticated ?? false)
            {
                var identity = (ClaimsIdentity)user.Identity;
                // On récupère le claim "roles" (qui est souvent un tableau JSON)
                var roleClaims = identity.FindAll("roles").ToList();

                if (roleClaims.Any())
                {
                    foreach (var roleClaim in roleClaims)
                    {
                        // Si Azure renvoie un tableau JSON ["Role1", "Role2"]
                        if (roleClaim.Value.StartsWith("["))
                        {
                            var roles = JsonSerializer.Deserialize<string[]>(roleClaim.Value);
                            if (roles != null)
                            {
                                foreach (var role in roles)
                                {
                                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                                }
                            }
                        }
                        else
                        {
                            // Si c'est une valeur simple
                            identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
                        }
                    }
                }
            }
            return user;
        }
    }
}
