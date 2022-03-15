using Sugcon.Foundation.Caching.Helpers;
using Sugcon.Foundation.Caching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.Caching.Events
{
    public class PublishEndEventHandler
    {
        protected void OnPublishEnd(object sender, EventArgs args)
        {
            string renderingHostEndPoint = ConfigurationHelper.GetSetting(Constants.SC_GENERATOR);
            HttpClient client = new HttpClient();
            var result = Task.Run(async () => await client.GetAsync(renderingHostEndPoint)).Result;
        }
    }
}
