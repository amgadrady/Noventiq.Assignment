using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NoventiqAssignment.API;
using NoventiqAssignment.DB.Context;
using NoventiqAssignment.DB.Models;
using System.Text;

namespace Noventiq.Assignment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddDbContext<NoventiqContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                        .AddEntityFrameworkStores<NoventiqContext>()
                        .AddDefaultTokenProviders();

            services.AddMedsultoServices(Configuration);

            services.AddAuthentication(
               option =>
               {
                   option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
               }
               ).AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                       .GetBytes(Configuration.GetSection("Token:Key").Value)),
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ClockSkew = TimeSpan.Zero
                   };
               });
            services.AddSwaggerGen(swagger =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put your JWT Bearer Token on textbox",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                swagger.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                                            {
                                                { jwtSecurityScheme, Array.Empty<string>() }
                                            }); swagger.SwaggerDoc("v1", new OpenApiInfo
                                            {
                                                Title = "Noventiq.Assignment",
                                                Description = "Noventiq APIS",
                                                Contact = new OpenApiContact() { Email = "amgadrady52@gmail,com" },
                                                Version = "1.0"
                                            });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Noventiq v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            ApplyDatabaseMigrations(app);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }

        private void ApplyDatabaseMigrations(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<NoventiqContext>();
                    context.Database.Migrate();


                    SeedData.Initialize(services).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                }
            }
        }
    }
}