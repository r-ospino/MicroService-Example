using FluentValidation;
using Flurl.Http.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Weelo.RafaelOspino.Api.Utils;
using Weelo.RafaelOspino.Commons.Mediatr;
using Weelo.RafaelOspino.Domain;
using Weelo.RafaelOspino.Infrastructure.ExternalServices;
using Weelo.RafaelOspino.SeedWork;

namespace Weelo.RafaelOspino.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Reads settings sections and inject as singletons.
            var cryptoCurrencyServiceSettings = configuration.GetSection("CryptoCurrencyServiceSettings").Get<CryptoCurrencyServiceSettings>();
            services.AddSingleton(cryptoCurrencyServiceSettings);

            var openIdConnectSettings = configuration.GetSection("OpenIdConnectSettings").Get<OpenIdConnectSettings>();
            services.AddSingleton(openIdConnectSettings);

            // Searializes enums as string.
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddMediatR(typeof(Startup));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IReadableRepository<CryptoCurrency>, CoinLoreTickersService>();

            // Adds validators
            services.AddValidatorsFromAssemblyContaining<Startup>();

            // Adds FlurlClient factory
            services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();

            // Adds Api Versioning
            ConfigureApiVersioning(services);

            // Adds Swagger
            ConfigureSwagger(services);

            // Adds Authentication services
            ConfigureAuthentication(services, openIdConnectSettings);

            services.AddHealthChecks()
                .AddCheck<CoinLoreHealthCheck>("CoinLoreHealthCheck")
                .AddCheck<OidcAuthorityHealthCheck>("OidcAuthorityHealthCheck");
        }

        private static void ConfigureApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(option =>
            {
                //option.DefaultApiVersion = new ApiVersion(1, 0);
                //option.AssumeDefaultVersionWhenUnspecified = false;
                option.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
                option.SubstituteApiVersionInUrl = true;
            });
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Adds generatd XML dcoumentation to Swagger JSON and UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                // Scheme configuration for JWT.
                var securityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http
                };

                // Adds authentication definition to the Swagger documentation.
                options.AddSecurityDefinition("Bearer", securityScheme);

                // Adds authentication requeriment to the Swagger test client.
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } });
            });

            // Defines Swagger document for each API version. 
            services.AddOptions<SwaggerGenOptions>().Configure<IApiVersionDescriptionProvider>((options, versionProvider) =>
            {
                foreach (ApiVersionDescription description in versionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo { Title = "CryptoCurrency", Version = description.ApiVersion.ToString() });
                }
            });
        }

        private static void ConfigureAuthentication(IServiceCollection services, OpenIdConnectSettings openIdConnectSettings)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                //ValidateIssuer = true,
                //ValidIssuer = "https://accounts.google.com",
                //IssuerSigningKey = new RsaSecurityKey(rsa),
                //CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = openIdConnectSettings.Authority;
                    options.Audience = "xxx";
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.RequireHttpsMetadata = false;

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/docs.json"; });
                app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "api-docs";
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/api-docs/{description.GroupName}/docs.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            //app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
