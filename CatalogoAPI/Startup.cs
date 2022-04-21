using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CatalogoAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using CatalogoAPI.Logging;
using CatalogoAPI.Repositories;
using CatalogoAPI.Repositories.Interfaces;
using CatalogoAPI.DTOs.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace CatalogoAPI
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
            var mappingConfig = new MapperConfiguration(
                cfg => { cfg.AddProfile(new MappingProfile());
                });
            
            IMapper mapper = mappingConfig.CreateMapper();

            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CatalogoDbContext>(
                opt => opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<CatalogoDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                    opt.TokenValidationParameters = new TokenValidationParameters 
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudience = Configuration["TOkenConfiguration:Audience"],
                        ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                    });

            services.AddSingleton(mapper);
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllers()
                    .AddNewtonsoftJson( opt => 
                        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore 
                );

            services.AddCors();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "CatalogoAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatalogoAPI v1"));
            }

  /*           loggerFactory.AddProvider(
                new CustomLogProvider(
                    new CustomLogProviderConfiguration 
                    {
                        LogLevel = LogLevel.Information
                    }
                )); */

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(c => 
                c.AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowAnyOrigin()
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
