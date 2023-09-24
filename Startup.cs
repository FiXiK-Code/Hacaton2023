using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MVP.Date;
using MVP.Date.Interfaces;
using MVP.Date.Repository;

namespace MVP
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            services.AddTransient<IProject, ProjectRep>();
            services.AddTransient<IMaterial, MaterialRep>();
            services.AddTransient<ITask, TaskRep>();
            services.AddTransient<IUser, UserRep>();



            services.AddDbContext<AppDB>(
                   options =>
                   {
                       options.UseMySql($"server=localhost;userid=root;pwd=root123456;port=3306;database=anilin_db");
                   });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddControllersWithViews();
        }

        
        public void Configure(IApplicationBuilder app, AppDB context)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();



            context.Database.Migrate();

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();

            app.UseMvcWithDefaultRoute();

            app.UseMvc(routs =>
            {
                routs.MapRoute(name: "default", template: "{controller=Home}/{action=Index}");

            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                AppDB content = scope.ServiceProvider.GetRequiredService<AppDB>();
                DBObjects.Initial(content);
            }
        }
    }

}
