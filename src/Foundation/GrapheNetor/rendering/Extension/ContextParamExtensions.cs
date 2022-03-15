using System;
using System.Collections.Generic;
using System.Text;
using Sugcon.Foundation.GrapheNetor.Rendering.Param;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Extension
{
    public static class ContextParamExtensions
    {
        public static string GenerateCacheKeyForContextParam(this ContextParam contextParam)
        {
            return $"{contextParam.Site}_{contextParam.Language}_{contextParam.RoutePath.Replace('/', '_')}".ToLower().Replace("__", "_");
        }
    }
}
