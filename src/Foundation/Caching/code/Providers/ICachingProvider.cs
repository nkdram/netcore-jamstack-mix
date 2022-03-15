using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.Caching.Providers
{
    public interface ICachingProvider
    {
        bool ContainsKey(string key);

        bool RemoveKey(string key);
    }
}
