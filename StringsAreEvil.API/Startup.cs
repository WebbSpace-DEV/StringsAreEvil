using Global.Utility.Options;
using Microsoft.AspNetCore.Mvc;

namespace StringsAreEvil.API;

public class Startup
{
    public IWebHostEnvironment Environment { get; }

    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        Environment = environment;
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddMvc()
            .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver())
            .SetCompatibilityVersion(CompatibilityVersion.Latest);

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        AppSettingsOption appSetting = new AppSettingsOption();
        Configuration.GetSection("AppSettings").Bind(appSetting);

        /*
         * 
         * Unfortunately, the configuration file needs to be accessed a second 
         * time in order to bind the option as a configuration instance.
         * 
         */
        services.Configure<AppSettingsOption>(Configuration.GetSection("AppSettings"));
    }

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

        app.UseDefaultFiles();
        app.UseStaticFiles();
    }
}
