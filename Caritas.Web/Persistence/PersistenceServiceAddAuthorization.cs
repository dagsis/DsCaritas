namespace Caritas.Web.Persistence
{
    public static class PersistenceServiceAddAuthorization
    {
        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                //opts.AddPolicy("PrivateAccess", policy =>
                //{
                //    policy.AddRequirements(new AllowPrivatePolicy());
                //});

                options.AddPolicy("ViewDashBoard", policy => policy.RequireClaim("ViewDashBoard", "True"));
                options.AddPolicy("CreateDashBoard", policy => policy.RequireClaim("CreateDashBoard", "True"));
                options.AddPolicy("EditDashBoard", policy => policy.RequireClaim("UpdateDashBoard", "True"));
                options.AddPolicy("DeleteDashBoard", policy => policy.RequireClaim("DeleteDashBoard", "True"));

                options.AddPolicy("ViewMaestros", policy => policy.RequireClaim("ViewMaestros", "True"));
                options.AddPolicy("CreateMaestros", policy => policy.RequireClaim("CreateMaestros", "True"));
                options.AddPolicy("EditMaestros", policy => policy.RequireClaim("UpdateMaestros", "True"));
                options.AddPolicy("DeleteMaestros", policy => policy.RequireClaim("DeleteMaestros", "True"));

                options.AddPolicy("ViewTablas", policy => policy.RequireClaim("ViewTablas", "True"));
                options.AddPolicy("CreateTablas", policy => policy.RequireClaim("CreateTablas", "True"));
                options.AddPolicy("EditTablas", policy => policy.RequireClaim("UpdateTablas", "True"));
                options.AddPolicy("DeleteTablas", policy => policy.RequireClaim("DeleteTablas", "True"));

                options.AddPolicy("ViewUsuarios", policy => policy.RequireClaim("ViewUsuarios", "True"));
                options.AddPolicy("CreateUsuarios", policy => policy.RequireClaim("CreateUsuarios", "True"));
                options.AddPolicy("EditUsuarios", policy => policy.RequireClaim("UpdateUsuarios", "True"));
                options.AddPolicy("DeleteUsuarios", policy => policy.RequireClaim("DeleteUsuarios", "True"));

                options.AddPolicy("ViewAdministracion", policy => policy.RequireClaim("ViewAdministracion", "True"));
                options.AddPolicy("CreateAdministracion", policy => policy.RequireClaim("CreateAdministracion", "True"));
                options.AddPolicy("EditAdministracion", policy => policy.RequireClaim("UpdateAdministracion", "True"));
                options.AddPolicy("DeleteAdministracion", policy => policy.RequireClaim("DeleteAdministracion", "True"));

            });
            return services;
        }
    }
}
