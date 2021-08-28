using Clean.Architecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace Clean.Architecture.Infrastructure
{
	public static class StartupSetup
	{
		public static void AddDbContext(this IServiceCollection services, string connectionString) =>
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlite(connectionString)); // will be created in web project root
    }
   
}
