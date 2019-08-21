using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TreasureDepartment.Data;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Logic.Deals.Services;
using TreasureDepartment.Logic.Friends.Services;
using TreasureDepartment.Logic.OfferCrud.Services;
using TreasureDepartment.Logic.Options;
using TreasureDepartment.Logic.Tokens.Services;
using TreasureDepartment.Logic.Users.Profiles;
using TreasureDepartment.Logic.Users.Services;
using TreasureDepartment.Web.Users.Profiles;

namespace TreasureDepartment.Web
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
            services.Configure<JWTOptions>(Configuration.GetSection(nameof(JWTOptions)));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JWTOptions>>().Value);
            services.AddDbContext<TreasuryDepartmentContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TreasuryDepartment")));


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<OfferCrudService<FriendInviteDbo>>();
            services.AddScoped<OfferCrudService<DealDbo>>();
            services.AddScoped<FriendService>();
            services.AddScoped<DealsService>();
            services.AddScoped<UserService>();
            services.AddScoped<TokenService>();


            var jwtOptions = new JWTOptions
            {
                Audience = Configuration[nameof(JWTOptions) + ":" + nameof(JWTOptions.Audience)],
                Issuer = Configuration[nameof(JWTOptions) + ":" + nameof(JWTOptions.Issuer)],
                Key = Configuration[nameof(JWTOptions) + ":" + nameof(JWTOptions.Key)],
                LifeTime = int.Parse(Configuration[nameof(JWTOptions) + ":" + nameof(JWTOptions.LifeTime)])
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtOptions.GetSymmetricSecurityKey(),
                    };
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Treasure-Department API"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Treasure-Department API"); });
        }
    }
}