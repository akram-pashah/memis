using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MEMIS.Models;
using Syncfusion.Blazor;
using Newtonsoft.Json;
namespace MEMIS
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddHttpContextAccessor();
      services.AddControllersWithViews()
        .AddRazorRuntimeCompilation()
        .AddNewtonsoftJson(options =>
        {
          options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
      services.AddDbContext<Data.AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
      services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<Data.AppDbContext>();
      services.AddCloudscribePagination();
      services.AddServerSideBlazor();
      services.AddSyncfusionBlazor();
      services.AddHealthChecks();
      services.AddDistributedMemoryCache();
      services.AddSession(options =>
      {
        options.IdleTimeout = TimeSpan.FromMinutes(20);
        options.Cookie.HttpOnly = true;
      });
      services.AddRazorPages();
      Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH5ecXRcQmBdUEB3XkU=");
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      //if (env.IsDevelopment())
      //{
      app.UseDeveloperExceptionPage();
      //}
      //else
      //{
      //    app.UseExceptionHandler("/Home/Error");
      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      //     app.UseHsts();
      //}
      //  app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSession();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Login}/{action=Index}/{id?}");
        //endpoints.MapRazorPages();
        endpoints.MapBlazorHub();
        //endpoints.MapFallbackToPage("/_Host");
      });


    }
  }
}
