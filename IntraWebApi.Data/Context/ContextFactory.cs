using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IntraWebApi.Data.Context
{
	public class ContextFactory : IDesignTimeDbContextFactory<Context>
	{
		public Context CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<Context>();
			optionsBuilder.UseSqlServer("Server=localhost:1401\\intrawebsql;Database=IntraDB;Trusted_Connection=True;ConnectRetryCount=0");
			return new Context(optionsBuilder.Options);
		}
	}
}
