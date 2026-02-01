using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using jbH60Store.Data;
using jbH60Store.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("MyConnection");
builder.Services.AddDbContext<H60AssignmentDB_jbContext>(x => x.UseSqlServer(connectionString));


var connectionIdentityString = builder.Configuration.GetConnectionString("jbH60StoreContextConnection");
builder.Services.AddDbContext<jbH60StoreContext>(x => x.UseSqlServer(connectionIdentityString));


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<jbH60StoreContext>();


builder.Services.AddHttpClient();

builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy",
    builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();
app.Run();
