using ClinicalPharmaSystem.DataContext;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// Configure your database connection here
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add the repository to the dependency injection container
builder.Services.AddTransient<DashboardRepository>(_ => new DashboardRepository(connectionString));

builder.Services.AddTransient<UserRepository>(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    return new UserRepository(connectionString, httpContextAccessor);
});

// Add the repository to the dependency injection container
builder.Services.AddTransient<PatientRepository>(_ => new PatientRepository(connectionString));

// Add the repository to the dependency injection container
builder.Services.AddTransient<MedicalTestRepository>(_ => new MedicalTestRepository(connectionString));

// Add the repository to the dependency injection container
builder.Services.AddTransient<AppointmentRepository>(_ => new AppointmentRepository(connectionString));

builder.Services.AddTransient<SettingsRepository>(_ => new SettingsRepository(connectionString));

builder.Services.AddTransient<PharmacyRepository>(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    return new PharmacyRepository(connectionString, httpContextAccessor);
});

builder.Services.AddTransient<ReportsRepository>(_ => new ReportsRepository(connectionString));

builder.Services.AddTransient<MenuViewComponent>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add session services
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.Use((context, next) =>
{
    return new RoleBasedMiddleware(next, "Home/Login", "Home/Registration","Doctor", "Admin", "Pharmacy", "Lab Technician").Invoke(context);
});

// Use session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}");

app.Run();
