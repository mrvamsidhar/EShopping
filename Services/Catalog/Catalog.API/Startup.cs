using AutoMapper.Internal;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

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
#pragma warning disable CS8604 // Possible null reference argument.
        services.AddHealthChecks()
            .AddMongoDb(Configuration["DatabaseSettings:ConnectionString"], "Catalog Mongo DB Health Check", HealthStatus.Degraded);
#pragma warning restore CS8604 // Possible null reference argument.
        services.AddApiVersioning();
        services.AddSwaggerGen(c => { 
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo{ Title = "Catalog.API", Version = "v1" }); 
        });
        services.AddAutoMapper(typeof(Startup));
        services.AddMediatR(cfg=> {
            cfg.RegisterServicesFromAssembly(typeof(CreateProductHandler).Assembly);
        });
        services.AddScoped<ICatalogContext, CatalogContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBrandRepository, ProductRepository>();
        services.AddScoped<ITypesRepository, ProductRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1");
            });
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => 
        { 
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            
        });
    }

}

