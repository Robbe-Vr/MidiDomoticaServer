using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MidiDomotica.Core.Handlers;
using MidiDomotica.Core.Handlers.Actions;
using MidiDomoticaApi.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomoticaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        private const string _salt = "FGF798DSF4SD";
        private static string ApiPassword;
        private static string ApiSecret;

        public static event EventHandler<PasswordChangeEventArgs> PasswordUpdate;

        internal static void SetUnhashedApiPassword(string pw)
        {
            if (!String.IsNullOrWhiteSpace(pw) && pw.Length > 4 &&
                  pw.Any(c => char.IsNumber(c)) && pw.Any(c => char.IsLower(c)) && pw.Any(c => char.IsUpper(c))
                )
            {
                string hashedPw = Hash(pw).ToUpper();

                SetApiPassword(hashedPw);
            }
        }

        private static string Hash(string pw)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pw + _salt);
            using (SHA512 hash = SHA512.Create())
            {
                byte[] hashedInputBytes = hash.ComputeHash(bytes);

                StringBuilder hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        private static void SetApiPassword(string pw)
        {
            ApiPassword = pw;
            Logger.Log("Web Api Password has been updated!");

            PasswordUpdate?.Invoke(new object(), new PasswordChangeEventArgs() { NewPassword = ApiPassword });
        }

        internal static void SetApiSecret(string secret)
        {
            ApiSecret = secret;
        }

        private const string midiDomoticaCorsPolicyName = "MidiDomoticaPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SetApiPassword(Configuration["Authorization:AppPassword"]);

            services.AddCors((options) =>
            {
                options.AddPolicy(midiDomoticaCorsPolicyName, builder =>
                {
                    builder.WithOrigins("http://localhost:55554", "http://192.168.2.101:55554")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            services.AddControllers();

            services.AddScoped(x =>
            {
                return new AuthorizationSettings() { AppSecret = ApiSecret, AppPassword = ApiPassword };
            });

            services.AddScoped<ControlsHandler>();
            services.AddScoped<ActionsHandler>();

            services.AddScoped<HueActionsHandler>();
            services.AddScoped<PowerModesActionsHandler>();
            services.AddScoped<KeyboardActionsHandler>();
            services.AddScoped<WindowsAudioActionsHandler>();
            services.AddScoped<SoundboardActionsHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(midiDomoticaCorsPolicyName);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class PasswordChangeEventArgs : EventArgs
    { 
        public string NewPassword { get; set; }
    }
}
