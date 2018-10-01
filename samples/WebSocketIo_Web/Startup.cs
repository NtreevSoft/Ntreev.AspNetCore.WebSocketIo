using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace WebSocketIo_Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcWithWebSocketIo();

            var jwtTokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["authentication:issuer"],
                ValidAudience = _configuration["authentication:issuer"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["authentication:key"]))
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearerWithWebSocketIo(options =>
                {
                    options.TokenValidationParameters = jwtTokenValidationParameters;
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets()
                .UseWebSocketIo()
                .UseAuthentication()
                .UseMvc();
        }
    }
}