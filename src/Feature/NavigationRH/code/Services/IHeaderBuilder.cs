using Sugcon.Feature.NavigationRH.Models;
using Sitecore.Data.Items;

namespace Sugcon.Feature.NavigationRH.Services
{
    public interface IHeaderBuilder
    {
        Header GetHeader(Item contextItem);
    }
}
