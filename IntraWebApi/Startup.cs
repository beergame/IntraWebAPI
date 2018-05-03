using IntraWebApi.Data.Context;
using IntraWebApi.Data.Repositories;
using IntraWebApi.Services.ArticleService;
using IntraWebApi.Services.UserService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntraWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();

            var connection = Configuration["ConnectionString"];

            services.AddDbContext<Context>(options => options.UseSqlServer(connection));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IArticleRepository, ArticleRepository>();
        }

       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }
    }
}
