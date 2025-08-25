using CarRentalAgencyMngSystem.Data;
using CarRentalAgencyMngSystem.Models;
using CarRentalAgencyMngSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using WebApCarRental.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CarRentalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarRentalConnection")));

builder.Services.AddTransient<ICar, CarRepo>();
builder.Services.AddTransient<ICustomer, CustomerRepo>();
builder.Services.AddTransient<IRental, RentalRepo>();
builder.Services.AddTransient<IPayment, PaymentRepo>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Set HomeController.Index as default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
