﻿using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Catalog.API;
public class Startup
{
    public IConfiguration Configuration;

    #pragma warning disable IDE0290
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    { 
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => 
        { 
            endpoints.MapControllers();            
        });
    }

}

