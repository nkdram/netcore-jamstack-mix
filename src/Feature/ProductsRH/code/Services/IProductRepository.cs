using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sugcon.Feature.ProductsRH.Services
{
    public interface IProductRepository
    {
        IEnumerable<Item> GetProducts(Item parent);
    }
}
