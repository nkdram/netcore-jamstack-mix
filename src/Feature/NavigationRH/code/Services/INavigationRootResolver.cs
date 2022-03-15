using Sitecore.Data.Items;

namespace Sugcon.Feature.NavigationRH.Services
{
    public interface INavigationRootResolver
    {
        Item GetNavigationRoot(Item contextItem);
    }
}
