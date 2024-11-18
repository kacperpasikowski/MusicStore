using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Project.Helpers;

namespace Project.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
			
			
			services.AddSingleton(provider => 
			{
				var cloudinarySettings = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
				var account = new Account(
					cloudinarySettings.CloudName,
					cloudinarySettings.ApiKey,
					cloudinarySettings.ApiSecret
				);
				return new Cloudinary(account);
			});
			
			
			return services;
		}
	}
}