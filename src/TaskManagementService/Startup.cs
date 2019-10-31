using Autofac;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Lionize.IntegrationMessages;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TIKSN.DependencyInjection;
using TIKSN.Lionize.Messaging;
using TIKSN.Lionize.Messaging.BackgroundServices;
using TIKSN.Lionize.Messaging.Options;
using TIKSN.Lionize.TaskManagementService.Business;
using TIKSN.Lionize.TaskManagementService.Data;
using TIKSN.Lionize.TaskManagementService.Hubs;
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

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(AllowSpecificCorsOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(opt =>
            {
                opt.MapControllers();
                opt.MapHub<MatrixHub>("/Hubs/Matrix");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new BusinessAutofacModule());
            builder.RegisterModule(new DataAutofacModule());
            builder.RegisterModule(new MessagingAutofacModule());

            builder.RegisterType<AccountService>()
                .As<IAccountService>()
                .SingleInstance();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                opt.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = false;
                o.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = false;
                o.SubstituteApiVersionInUrl = true;
            });

            var servicesConfigurationSection = Configuration.GetSection("Services");
            services.Configure<ServiceDiscoveryOptions>(servicesConfigurationSection);

            var serviceDiscoveryOptions = new ServiceDiscoveryOptions();
            servicesConfigurationSection.Bind(serviceDiscoveryOptions);

            var webApiResourceOptions = new WebApiResourceOptions();
            Configuration.GetSection("ApiResource").Bind(webApiResourceOptions);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = $"{serviceDiscoveryOptions.Identity.BaseAddress}";
                options.RequireHttpsMetadata = false;

                options.ApiName = webApiResourceOptions.ApiName;
                options.ApiSecret = webApiResourceOptions.ApiSecret;

                options.JwtBearerEvents.OnMessageReceived = context =>
                {
                    if (context.Request.Query.TryGetValue("access_token", out StringValues token) && context.Request.Path.StartsWithSegments("/hubs", StringComparison.OrdinalIgnoreCase))
                    {
                        //context.Options.Authority = $"{serviceDiscoveryOptions.Identity.BaseAddress}";
                        context.Token = token.Single();
                        //context.Options.Validate();
                    }

                    return Task.CompletedTask;
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                options.MapType<PathString>(() => new OpenApiSchema { Type = "string" });
                options.MapType<Type>(() => new OpenApiSchema { Type = "string" });

                options.SwaggerDoc("1.0", new OpenApiInfo { Title = "Lionize / Task Management Service", Version = "1.0" });

                var def = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                options.AddSecurityDefinition("Bearer", def);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {def, new List<string>()}
                });
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
                        cpbuilder.AllowCredentials();
                        cpbuilder.WithOrigins(origins);
                    }
                });
            });

            services.Configure<AccountOptions>(Configuration.GetSection("Account"));
            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("Services"));

            services.AddAutoMapper((provider, exp) =>
            {
                var bigIntegerTypeConverter = provider.GetRequiredService<BigIntegerTypeConverter>();
                exp.CreateMap<BigInteger, string>().ConvertUsing(bigIntegerTypeConverter);
                exp.CreateMap<string, BigInteger>().ConvertUsing(bigIntegerTypeConverter);
                exp.AddProfile(new BusinessMappingProfile());
            }, typeof(WebApiMappingProfile), typeof(DataMappingProfile));

            services.AddSingleton((IConfigurationRoot)Configuration);

            services.AddFrameworkPlatform();
            services.AddMediatR(typeof(BusinessAutofacModule).GetTypeInfo().Assembly);

            services.Configure<ApplicationOptions>(opt =>
            {
                opt.ApplictionId = "TaskManagementService";
                opt.ApplictionQueuePart = "task_management";
            });

            services.AddHostedService<ConsumerBackgroundService<TaskUpserted>>();

            services.AddSignalR(opt => opt.EnableDetailedErrors = true);
        }
    }
}
