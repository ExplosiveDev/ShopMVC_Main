using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using DataAccess.Entities;
using ShopMVC;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using ShopMVC.Services;
using ShopMVC.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Core.Types;
using DataAccess.Interfaces;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
//get connection string
string connection = builder.Configuration.GetConnectionString("ShopMVCConnection") ?? throw new InvalidOperationException("Connection string 'ShopMVCConnection' not found.");

//get remote connection string
//string connection = builder.Configuration.GetConnectionString("RemoteDBSomeeCom") ?? throw new InvalidOperationException("Connection string 'RemoteDBSomeeCom' not found.");

//add contect WebAppLibraryContext as service by application
builder.Services.AddDbContext<ShopMVCDbContext>(options => options.UseSqlServer(connection));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<ShopMVCDbContext>();

//add Fluent Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

//add Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    
    options.IdleTimeout = TimeSpan.FromSeconds(1000);
    options.Cookie.Name = "_ShopMVC.Session";
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;  
});

//Dependences Injection Remote Services
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IProductShowService, ProductShowService>();
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddHttpContextAccessor();

//add Repository for all entities
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//Auto Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

//Seed Roles
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    Seeder.SeedRoles(services).Wait();
    Seeder.SeedAdmin(services).Wait();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
