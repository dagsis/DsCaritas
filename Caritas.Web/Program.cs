using Caritas.Web.Persistence;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddPersistenceServices(builder.Configuration);

//builder.Services.AddTransient<IAuthorizationHandler, AllowPrivateHandler>();
builder.Services.AddAuthorization(opts =>
{
    //opts.AddPolicy("PrivateAccess", policy =>
    //{
    //    policy.AddRequirements(new AllowPrivatePolicy());
    //});

    opts.AddPolicy("ViewClientes", policy => policy.RequireClaim("ViewClientes", "True"));

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
