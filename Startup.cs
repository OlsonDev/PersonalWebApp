using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalWebApp.Conventions;
using PersonalWebApp.Middleware;
using PersonalWebApp.Models.Db;
using PersonalWebApp.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Diagnostics;

namespace PersonalWebApp {
	public class Startup {
		public Startup(IHostingEnvironment env) {
			var builder = new ConfigurationBuilder()
					.SetBasePath(env.ContentRootPath)
					.AddJsonFile("appsettings.json")
					.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
					.AddUserSecrets()
					.AddEnvironmentVariables()
			;
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services) {
			services.AddEntityFramework()
				.AddEntityFrameworkSqlServer()
				.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]))
			;
			services.AddMemoryCache();
			services.AddSession(options => {
				options.IdleTimeout = TimeSpan.FromDays(14);
			});

			services.AddMvc(options => {
				//options.Conventions.Add(new HyphenatedRoutingConvention());
			});

			services.AddSingleton<IConfiguration>(sp => Configuration);

			// These should be request scoped so DbContext leases a new connection and doesn't close it on other requests
			services
				.AddScoped<SkillService>()
				.AddScoped<BlogService>()
			;
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.EnsureSampleData(); // TODO: Only in production
			} else {
				app.UseExceptionHandler("/error");
				try {
					using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()) {
						serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
					}
					app.EnsureSampleData();
				} catch {
					// TODO: Should this be handled? Snippet copy-pasted from Microsoft example
					// http://docs.asp.net/en/latest/conceptual-overview/understanding-aspnet5-apps.html
				}
			}

			app.UseStaticFiles();
			app.UseStripWhitespace();
			app.UseSession();
			app.UseMvc(routes => {
				routes.MapRoute("default", "{controller=Blog}/{action=Index}/{id?}");
			});
		}

		public static void Main(string[] args) {
			var host = new WebHostBuilder()
					.UseKestrel()
					.UseContentRoot(Directory.GetCurrentDirectory())
					.UseIISIntegration()
					.UseStartup<Startup>()
					.Build();

			host.Run();
		}
	}
}