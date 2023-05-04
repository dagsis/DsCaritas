using Caritas.Web.Persistence;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddPersistenceServices(builder.Configuration);

//builder.Services.AddTransient<IAuthorizationHandler, AllowPrivateHandler>();
builder.Services.AddAuthorization(options =>
{
    //opts.AddPolicy("PrivateAccess", policy =>
    //{
    //    policy.AddRequirements(new AllowPrivatePolicy());
    //});

    options.AddPolicy("ViewDashBoard", policy => policy.RequireClaim("ViewDashBoard", "True"));
    options.AddPolicy("VerBotonDashBoard", policy => policy.RequireClaim("CreateDashBoard", "True"));
    options.AddPolicy("EditDashBoard", policy => policy.RequireClaim("UpdateDashBoard", "True"));
    options.AddPolicy("DeleteDashBoard", policy => policy.RequireClaim("DeleteDashBoard", "True"));

    options.AddPolicy("ViewMaestros", policy => policy.RequireClaim("ViewMaestros", "True"));
    options.AddPolicy("VerBotonMaestros", policy => policy.RequireClaim("CreateMaestros", "True"));
    options.AddPolicy("EditMaestros", policy => policy.RequireClaim("UpdateMaestros", "True"));
    options.AddPolicy("DeleteMaestros", policy => policy.RequireClaim("DeleteMaestros", "True"));

    options.AddPolicy("ViewTablas", policy => policy.RequireClaim("ViewTablas", "True"));
    options.AddPolicy("VerBotonTablas", policy => policy.RequireClaim("CreateTablas", "True"));
    options.AddPolicy("EditTablas", policy => policy.RequireClaim("UpdateTablas", "True"));
    options.AddPolicy("DeleteTablas", policy => policy.RequireClaim("DeleteTablas", "True"));

    options.AddPolicy("ViewUsuarios", policy => policy.RequireClaim("ViewUsuarios", "True"));
    options.AddPolicy("VerBotonUsuarios", policy => policy.RequireClaim("CreateUsuarios", "True"));
    options.AddPolicy("EditUsuarios", policy => policy.RequireClaim("UpdateUsuarios", "True"));
    options.AddPolicy("DeleteUsuarios", policy => policy.RequireClaim("DeleteUsuarios", "True"));

    options.AddPolicy("ViewAdministracion", policy => policy.RequireClaim("ViewAdministracion", "True"));
    options.AddPolicy("VerBotonAdministracion", policy => policy.RequireClaim("CreateAdministracion", "True"));
    options.AddPolicy("EditAdministracion", policy => policy.RequireClaim("UpdateAdministracion", "True"));
    options.AddPolicy("DeleteAdministracion", policy => policy.RequireClaim("DeleteAdministracion", "True"));

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
