using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace GestionStock.Client.Security
{
    public class JwtAuthenticationStateProvider(IJSRuntime js, HttpClient httpClient) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // recupérer mon token // localstorage
            string? token = await js.InvokeAsync<string?>("localStorage.getItem", "token");
            httpClient.DefaultRequestHeaders.Remove("Authorization");

            if(token == null)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            // decoder le token pour créer un authenticationState qui contiendra les claims du token
            JwtSecurityTokenHandler handler = new();

            var claims = handler.ReadJwtToken(token).Claims;
            var identity = new ClaimsIdentity(claims, "jwt");
            var principal = new ClaimsPrincipal(identity);

            return new AuthenticationState(principal);

        }


        public async Task Login(string token)
        {
            await js.InvokeVoidAsync("localStorage.setItem", "token", token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            await js.InvokeVoidAsync("localStorage.removeItem", "token");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public static class UserExtensions
    {
        public static bool HasAnyRoles(this ClaimsPrincipal user, params string[] roles)
        {
            return roles.Any(user.IsInRole);
        }
    }
}


