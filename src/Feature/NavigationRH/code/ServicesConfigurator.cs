using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Sugcon.Feature.NavigationRH
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.IHeaderBuilder, Services.HeaderBuilder>();
            serviceCollection.AddTransient<Services.INavigationRootResolver, Services.NavigationRootResolver>();
        }
    }
}