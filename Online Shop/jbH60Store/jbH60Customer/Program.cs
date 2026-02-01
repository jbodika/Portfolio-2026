using Microsoft.EntityFrameworkCore;
using jbH60Customer.Models;
using Microsoft.AspNetCore.Identity;
using jbH60Customer.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
var connectionString = builder.Configuration.GetConnectionString("MyConnection");
builder.Services.AddDbContext<H60AssignmentDB_jbContext>(x => x.UseSqlServer(connectionString));


var connectionIdentityString = builder.Configuration.GetConnectionString("jbH60CustomerContextConnection");
builder.Services.AddDbContext<jbH60CustomerContext>(x => x.UseSqlServer(connectionIdentityString));





builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<jbH60CustomerContext>();




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

app.MapRazorPages();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
