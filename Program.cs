using ClinicalPharmaSystem.Controllers;
using ClinicalPharmaSystem.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure your database connection here
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add the repository to the dependency injection container
builder.Services.AddTransient<DashboardRepository>(_ => new DashboardRepository(connectionString));

// Add the repository to the dependency injection container
builder.Services.AddTransient<UserRepository>(_ => new UserRepository(connectionString));

// Add the repository to the dependency injection container
builder.Services.AddTransient<PatientRepository>(_ => new PatientRepository(connectionString));

// Add the repository to the dependency injection container
builder.Services.AddTransient<MedicalTestRepository>(_ => new MedicalTestRepository(connectionString));

// Add the repository to the dependency injection container
builder.Services.AddTransient<AppointmentRepository>(_ => new AppointmentRepository(connectionString));

var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}");

app.Run();
