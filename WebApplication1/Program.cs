using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interface;
using WebApplication1.Models;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<DbaseContext>();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbaseContext") ?? throw new InvalidOperationException("Connection string not found."))
);


builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DbaseContext>().AddDefaultTokenProviders();
//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<DbaseContext>().AddDefaultTokenProviders();

//builder.Services.AddAuth0WebAppAuthentication(options => {
//        options.Domain = builder.Configuration["Auth0:Domain"];
//        options.ClientId = builder.Configuration["Auth0:ClientId"];
//        options.Scope = "openid profile email";
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "API",
    pattern: "{area:exists}/{controller}/{action}");


//app.MapControllerRoute(
//    name: "api",
//    pattern: "/API/{controller}/{action}/{id?}");

//endpoints.MapAreaControllerRoute(
//    name: "Department",
//    areaName: "Department",
//    pattern: "Department/{controller=Sales}/{action=Index}"
//);

app.Run();
