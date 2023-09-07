using NewNew.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddDbContext<MVCDemoContext>(options =>
			options.UseSqlServer(Configuration.GetConnectionString("MvcDemoConnectionString")));

		services.AddLogging();
		services.AddSingleton<IConfiguration>(Configuration);
		services.AddSingleton<IHostedService, NotificationService>();

		services.AddControllersWithViews();

		services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.Cookie.Name = "YourCookieName";
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.LoginPath = "/MemberAccount/Login"; // Specify the login page URL
				options.LogoutPath = "/MemberAccount/Logout"; // Specify the logout page URL
			});

		services.AddDistributedMemoryCache(); // Required for session state
		services.AddSession(options =>
		{
			options.Cookie.Name = "YourSessionCookieName"; // Set a unique name for the session cookie
			options.IdleTimeout = TimeSpan.FromMinutes(20); // Set the session timeout duration
			options.Cookie.HttpOnly = true; // Ensure the session cookie is only accessed via HTTP
		});
		services.AddHttpContextAccessor();
	}


	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseSession(); // Enable session middleware

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Admin}/{action=Home}/{id?}");
		});
	}
}

public class Program
{
	public static void Main(string[] args)
	{
		CreateHostBuilder(args).Build().Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseKestrel(options =>
				{
					options.Limits.MaxRequestBodySize = null;
				});
				webBuilder.UseStartup<Startup>();
			});
}
