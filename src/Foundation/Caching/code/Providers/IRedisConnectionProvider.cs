using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.Caching.Providers
{
    public interface IRedisConnectionProvider
    {
        IRedisClient GetRedisClient();
    }
}
