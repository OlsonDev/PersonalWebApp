using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PersonalWebApp {
	public class Startup {
		public Startup(IHostingEnvironment env) {
			var builder = new ConfigurationBuilder()
					.AddJsonFile("appsettings.json")
					.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
			;
			if (env.IsDevelopment()) {
				builder.AddUserSecrets();
			}
			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}
		public IConfigurationRoot Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services) {
			services.AddMvc();
			services.AddSingleton<IConfiguration>(sp => Configuration);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/error");
			}
			app.UseIISPlatformHandler();
			app.UseStaticFiles();
			app.UseMvc(routes => {
				routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}

		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
	}
}