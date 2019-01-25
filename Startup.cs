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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ClaimsApi.Models;
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",new Info 
                {
                    Version = "v1", 
                    Title = "Claims API",
                    Description = "An example ASP.NET Core Web API that retrieves medical claims records from a SQL server database",
                    TermsOfService = "Education purposes only",
                    Contact = new Contact
                    {
                        Name = "Microsoft",
                        Url = "https://github.com/ganrad/aks-aspnet-sqldb-rest"
                    },
                    License = new License
                    {
                        Name = "Apache 2.0",
                        Url = "https://www.apache.org/licenses/LICENSE-2.0"
                    }                    
                });

                 // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseMvc();
        }
    }
}
