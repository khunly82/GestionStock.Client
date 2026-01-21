using GestionStock.Client;
using GestionStock.Client.Security;
using GestionStock.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://demogestionstockapi-cgdrb9bpa5d3dfht.westeurope-01.azurewebsites.net") });

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddSingleton<LoadingStateService>();

 ///builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

builder.Services.AddMsalAuthentication(options =>
{

    options.ProviderOptions.Authentication.Authority = "https://login.microsoftonline.com/70d1d84d-d851-4dbf-8b45-1fd01c24ee60";
    options.ProviderOptions.Authentication.ClientId = "1314b8e2-e176-4cd4-8ab9-7560ef29d4b9";
    options.ProviderOptions.Authentication.ValidateAuthority = true;

    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://07459912-8c28-474a-ac71-e272bdfb44cc/user_access");
})
.AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, CustomUserFactory>();

await builder.Build().RunAsync();
