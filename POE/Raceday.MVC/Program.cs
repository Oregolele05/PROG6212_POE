using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. Add MVC Services
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<RaceDay.MVC.Services.ApiService>();

// 2. Add Session to the MVC App
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. REGISTER THE COOKIE CONTAINER AS A SINGLETON
builder.Services.AddSingleton<System.Net.CookieContainer>();

// 4. Configure HttpClient to talk to the API
builder.Services.AddHttpClient("RaceDayApi", client =>
{
    // If running in Docker, use the service name. Otherwise, use localhost.
    var apiUrl = Environment.GetEnvironmentVariable("ApiBaseUrl") ?? "https://localhost:7266/";
    client.BaseAddress = new Uri(apiUrl);
})
.ConfigurePrimaryHttpMessageHandler(serviceProvider =>
{
    return new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = serviceProvider.GetRequiredService<System.Net.CookieContainer>()
    };
})


.ConfigurePrimaryHttpMessageHandler(serviceProvider =>
{
    return new HttpClientHandler
    {
        UseCookies = true,
        // USE THE SINGLETON COOKIE CONTAINER HERE
        CookieContainer = serviceProvider.GetRequiredService<System.Net.CookieContainer>()
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 5. Use Session before Authorization
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();