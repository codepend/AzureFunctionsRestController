using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionsRestController.Startup))]

namespace FunctionsRestController
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
           // builder.Services.AddHttpClient();

            builder.Services.AddSingleton<UserRepository>();
            

            //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}