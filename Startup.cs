/**
 * Author : Ganesh Radhakrishnan (ganrad01@gmail.com)
 *
 * Notes:
 * ID11092019: Upgraded application artifacts to use .Net Core 3.0
 * ID11142019: Updated application to use Swashbuckle v5 - 5.0.0-rc4
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
// using Microsoft.AspNetCore.Mvc; ID11092019.o
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ClaimsApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Hosting; // ID11092019.n
using Microsoft.OpenApi.Models; // ID11142019.n

namespace ClaimsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<ClaimsContext>(opt => opt.UseInMemoryDatabase("ClaimItems"));
            services.AddDbContext<ClaimsContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("SqlServerDb")));
            services.AddControllers();
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2); ID11092019.o

            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",new OpenApiInfo 
                {
                    Version = "v1", 
                    Title = "Claims API",
                    Description = "An example ASP.NET Core Web API that retrieves medical claims records from a SQL server database",
                    TermsOfService = new Uri("https://github.com/ganrad/aks-aspnet-sqldb-rest"),
                    Contact = new OpenApiContact
                    {
                        Name = "Microsoft",
			Email = "garadha@microsoft.com",
                        Url = new Uri("https://github.com/ganrad/aks-aspnet-sqldb-rest")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Apache 2.0",
                        Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0")
                    }            
                });

                 // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
	    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // public void Configure(IApplicationBuilder app, IHostingEnvironment env) ID11092019.o
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // ID11092019.n
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            
            // Enable middleware to serve generated Swagger as a JSON end-point
            app.UseSwagger();
            // Enable middleware to serve Swagger-ui, specifying Swagger JSON end-point.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Claims API");
            });
	    
             
            // app.UseHttpsRedirection();
            // Add MVC middleware
            // app.UseMvc(); ID11092019.o
 
            // ID11092019.sn
            app.UseRouting();
            app.UseEndpoints(endpoints =>
	    {
		endpoints.MapControllers();
	    });
            // ID11092019.en
        }
    }
}
