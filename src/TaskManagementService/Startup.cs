﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using TIKSN.DependencyInjection;
using TIKSN.Lionize.TaskManagementService.Business;
using TIKSN.Lionize.TaskManagementService.Options;
using TIKSN.Lionize.TaskManagementService.Services;

namespace TIKSN.Lionize.TaskManagementService
{
    public class Startup
    {
        private readonly string AllowSpecificCorsOrigins = "_AllowSpecificCorsOrigins_";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/1.0/swagger.json", "API 1.0");
            });

            app.UseCors(AllowSpecificCorsOrigins);

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning();
            services.AddVersionedApiExplorer();

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(Configuration.GetConnectionString("RabbitMQ")), hostConfigurator =>
                    {
                    });
                }));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("1.0", new OpenApiInfo { Title = "Lionize / Task Management Service", Version = "1.0" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificCorsOrigins,
                cpbuilder =>
                {
                    var origins = Configuration.GetSection("Cors").GetSection("Origins").Get<string[]>();

                    if (origins != null)
                    {
                        cpbuilder.AllowAnyMethod();
                        cpbuilder.AllowAnyHeader();
                        cpbuilder.WithOrigins(origins);
                    }
                });
            });

            services.Configure<AccountOptions>(Configuration.GetSection("Account"));
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("Services"));

            services.AddAutoMapper((provider, exp) =>
            {
                exp.AddProfile(new BusinessMappingProfile());
            }, typeof(WebApiMappingProfile));

            services.AddSingleton((IConfigurationRoot)Configuration);

            services.AddFrameworkPlatform();
            services.AddMediatR(typeof(BusinessAutofacModule).GetTypeInfo().Assembly);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            ConfigureContainer(builder);

            return new AutofacServiceProvider(builder.Build());
        }

        private void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>()
                .As<IAccountService>()
                .SingleInstance();
        }
    }
}