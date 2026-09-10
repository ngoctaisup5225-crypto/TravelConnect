using Microsoft.EntityFrameworkCore;
using TravelConnect.Data;

var builder = WebApplication.CreateBuilder(args);


// =============================
// MVC
// =============================

builder.Services.AddControllersWithViews();


// =============================
// DATABASE
// =============================

builder.Services.AddDbContext<TravelConnectDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// =============================
// SESSION
// =============================

builder.Services.AddSession();


var app = builder.Build();


// =============================
// HTTP PIPELINE
// =============================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();


// =============================
// ROUTE
// =============================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();