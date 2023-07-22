using Caritas.Insfrastructure.Data;
using Caritas.Web.Services;
using DsCommon;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Caritas.Web.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpClient();
            services.AddHttpContextAccessor();


            var cnnString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(cnnString));


            services.AddAutoMapper(typeof(Program));

            SDRutas.GateWayAPIBase = configuration["ApiGatewayUrl"];
            SDRutas.IdentityApiBase = configuration["AuthenticationUrl"];
            SDRutas.EmailApiBase = configuration["ApiEmailUrl"];
            SDRutas.CompaniaId = Convert.ToInt32(configuration["Sistema:CompaniaId"]);
            SDRutas.AplicacionId = Convert.ToInt32(configuration["Sistema:AplicacionId"]);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBaseService, BaseService>();

            services.AddTransient<IServiceManagement, ServiceManagement>();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                     .AddCookie();

            return services;
        }
    }
}
