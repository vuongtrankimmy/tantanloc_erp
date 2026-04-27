using TTL.HRM.WebApp.Components;

// Prevent IIS Express ThreadPool starvation deadlock when components make local HTTP requests
ThreadPool.SetMinThreads(100, 100);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>().BaseUri) });
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
// app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(TTL.HRM.Client.Pages.Index.Index).Assembly, typeof(TTL.Auth.Client.Pages.Login.Account.Account).Assembly, typeof(TTL.WMS.Client.Pages.Suppliers.SupplierList).Assembly);

app.Run();
