using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VMS.Client;
using VMS.Client.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using VMS.Client.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddServices(builder.Configuration);


await builder.Build().RunAsync();
