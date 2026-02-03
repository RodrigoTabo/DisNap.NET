using DisnApp.Data;
using DisnApp.Models;
using DisnApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Incluir contexto
builder.Services.AddDbContext<RedDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RedDbContext")));
builder.Services.AddScoped<IMensajeService, MensajeService>();
builder.Services.AddScoped<IHistoriaService, HistoriaService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IPublicacionService, PublicacionService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();



//add identity core
builder.Services
    .AddDefaultIdentity<Usuario>(
    options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;

        //Password
        options.Password.RequiredLength = 3;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;

        //SignIn
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;


        //User
        options.User.RequireUniqueEmail = true;


        //Bloqueo
        //Cuanto bloqueo le podemos dar a una cuenta.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(0);
        //Despues de cuanto intentos le damos el bloqueo?
        options.Lockout.MaxFailedAccessAttempts = 1;
        //Vamos a permitir que las cuentas nuevas se bloqueen?
        options.Lockout.AllowedForNewUsers = true;


    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<RedDbContext>();
builder.Services.AddRazorPages();


builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Login");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Register");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ForgotPassword");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/AccessDenied");
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();      // 1) primero


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();


app.Run();
