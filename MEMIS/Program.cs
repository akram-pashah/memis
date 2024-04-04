using MEMIS;
using Microsoft.AspNetCore.Hosting;

internal class Program
{
  public static void Main(string[] args)
  {
    CreateHostBuilder(args).Build().Run();
  }
  
  public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                 webBuilder.UseStartup<Startup>();
               });
  //private static void Main(string[] args)
  //{
  //  var builder = WebApplication.CreateBuilder(args);

  //  builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

  //  // Add services to the container.
  //  builder.Services.AddRazorPages();

  //  var app = builder.Build();

  //  // Configure the HTTP request pipeline.
  //  if (!app.Environment.IsDevelopment())
  //  {
  //    app.UseExceptionHandler("/Error");
  //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  //    app.UseHsts();
  //  }

  //  app.UseHttpsRedirection();
  //  app.UseStaticFiles();

  //  app.UseRouting();

  //  app.UseAuthorization();

  //  app.MapRazorPages();

  //  app.Run();
  //}
}
