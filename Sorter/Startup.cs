using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sorter.Filters;
using Sorter.Helpers;
using Sorter.Middlewares;
using Sorter.Repositories;
using Sorter.Services;

namespace Sorter
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers(options => options.Filters.Add<InputValidationFilter>());

      RegisterHelpers(services);
      RegisterRepositories(services);
      RegisterServices(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseMiddleware<ExceptionMiddleware>();

      app.UseRouting();
      app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
    }

    private static void RegisterHelpers(IServiceCollection services)
    {
      services.AddSingleton<IFileHelper, FileHelper>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
      services.AddSingleton<IStorageRepository, StorageRepository>();
    }

    private static void RegisterServices(IServiceCollection services)
    {
      services.AddSingleton<ISortService, SortService>();
      services.AddSingleton<IStorageService, StorageService>();
    }
  }
}
