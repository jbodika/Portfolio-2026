using jbH60Services.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL;
//using StoreLibrary.DAL;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;
//using StoreLibrary.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MyConnection");
builder.Services.AddDbContext<H60AssignmentDB_jbContext>(options => options.UseSqlServer(connectionString));



//var connectionIdentityString = builder.Configuration.GetConnectionString("jbH60StoreContextConnection");
//builder.Services.AddDbContext<jbH60StoreContext>(x => x.UseSqlServer(connectionIdentityString));


//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<jbH60StoreContext>();


builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();




builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy",
      builder => builder.WithOrigins("http://localhost:59995")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.UseCors("CorsPolicy");

app.Run();
