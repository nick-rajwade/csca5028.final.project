using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;
using csca5028BlazorApp2.Data;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri("https://csca5028vault.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
Console.WriteLine($"keyvaultEndPoint: {keyVaultEndpoint}");

// Add services to the container.
builder.Services.AddSyncfusionBlazor();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<RedisCachingService>();
builder.Services.AddSingleton<MemoryCachingService>();
builder.Services.AddHostedService<SalesBackgroundService>();
//builder.Services.AddDistributedRedisCache(option =>
//{
 //   option.Configuration = builder.Configuration["CacheConnection"];
//});

var app = builder.Build();
//Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["syncfusion"]);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
