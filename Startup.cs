using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OTTMyPlatform.Repository;
using OTTMyPlatform.Repository.Interface;
using OTTMyPlatform.Repository.Interface.Context;
using System.Text;

namespace OTTMyPlatform
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration nconfiguration)
        {
            configuration = nconfiguration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "rpn_engine", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description =  "Please enter valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
            {
                opts.SaveToken = true;
                opts.RequireHttpsMetadata = true;
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWTConfig:Issuer"],
                    ValidAudience = configuration["JWTConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfig:key"])),
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            //var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            services.AddCors(options =>
            {
                options.AddPolicy("_myAllowSpecificOrigins", p =>
                {
                    p.WithOrigins("http://localhost:3000","http://localhost:3000/login", "https://localhost:3000/login")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
            services.AddTransient<IDBContext,DBContext>();
            services.AddTransient<IAdminRepository,AdminRepository>();
            services.AddTransient<IUserRepository,UserRepository>();
            services.AddTransient<ITVShowImageProcessRepository,TVShowImageProcessRepository>();
            services.AddTransient<IAuthRepository,AuthRepository>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "rpn_engine v1"));
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("_myAllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
