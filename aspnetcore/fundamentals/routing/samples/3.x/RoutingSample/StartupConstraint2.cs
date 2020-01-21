using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;

namespace RoutingSample
{
	public class StartupConstraint2
	{
		public StartupConstraint2(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		#region snippet
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddRouting(options =>
			{
				options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
			});
		}
		#endregion

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}

	#region snippet2
	public class SlugifyParameterTransformer : IOutboundParameterTransformer
	{
		public string TransformOutbound(object value)
		{
			if (value == null) { return null; }

			return Regex.Replace(value.ToString(), 
								 "([a-z])([A-Z])", "$1-$2").ToLower();
		}
	}
	#endregion
}
