var builder = WebApplication.CreateBuilder(args);
var useHttpsRedirection = builder.Configuration.GetValue<bool>("UseHttpsRedirection");

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    if (useHttpsRedirection)
    {
        app.UseHsts();
    }
}

if (useHttpsRedirection)
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.Use(async (context, next) =>
{
    string path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
    bool isLoginPath = path.StartsWith("/account/login");
    bool isLogOffPath = path.StartsWith("/account/logoff");
    bool isStaticPath = path.StartsWith("/content") || path.StartsWith("/scripts") || path.StartsWith("/fonts") || path.StartsWith("/favicon");

    if(isLoginPath || isLogOffPath || isStaticPath) {
        await next();
        return;
    }

    bool isAuthenticated = string.Equals(context.Session.GetString("SimpleAuth.Authenticated"), "true", StringComparison.OrdinalIgnoreCase);
    if(isAuthenticated) {
        await next();
        return;
    }

    string returnUrl = context.Request.Path + context.Request.QueryString;
    context.Response.Redirect($"/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
