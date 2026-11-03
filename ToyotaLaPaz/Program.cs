using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToyotaLaPazContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vehiculos}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
