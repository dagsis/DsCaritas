using Caritas.Common.Models;
using Caritas.Insfrastructure;
using Caritas.Insfrastructure.Data;
using Caritas.Web.Persistence;
using Caritas.Web.Services;
using DsCommon.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddPersistenceServices(builder.Configuration);

 string[] myUserList = { "Carlos D' Agostino", "user2", "user3" };


//builder.Services.AddAuthorization(options =>
// {

//     options.AddPolicy("ViewClientes",
//               policy => policy.RequireAssertion(
//                     context => myUserList.Contains(context.User.Identity.Name)));

//     //options.AddPolicy("ViewClientes", policy => policy.RequireClaim("ViewClientes", "True"));
//     //  options.AddPolicy("ViewClientes", policy => policy.RequireUserName("Carlos D' Agostino")); 
// });

//builder.Services.AddTransient<IAuthorizationHandler, AllowPrivateHandler>();
builder.Services.AddAuthorization(opts =>
{
    //opts.AddPolicy("PrivateAccess", policy =>
    //{
    //    policy.AddRequirements(new AllowPrivatePolicy());
    //});

    opts.AddPolicy("ViewClientes", policy => policy.RequireClaim("ViewClientes", "True"));

    //opts.AddPolicy("AddEditCliente", policy => {
    //    policy.RequireClaim("ViewClientes", "True");
    //    policy.RequireClaim("Add User", "Add User");
    //    policy.RequireClaim("Edit User", "Edit User");
    //});
    //opts.AddPolicy("DeleteUser", policy => policy.RequireClaim("Delete User", "Delete User"));
});



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");

IList<CultureInfo> sc = new List<CultureInfo>();
sc.Add(new CultureInfo("es-MX"));



var lo = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-MX"),
    SupportedCultures = sc,
    SupportedUICultures = sc
};

var cp = lo.RequestCultureProviders.OfType<CookieRequestCultureProvider>().First();
cp.CookieName = "UserCulture"; // Or whatever name that you like
app.UseRequestLocalization(lo);


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
