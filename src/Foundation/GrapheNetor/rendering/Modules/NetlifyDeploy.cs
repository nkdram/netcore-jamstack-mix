using NetlifySharp;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Modules
{
    public class NetlifyDeploy : Module
    {

        protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            var site = await Task.Run(() => new NetlifyClient("5y2Eds6n6QVGLEsQtc3CzaqLI3VA84q7SDwsdIfg3ck", new System.Net.Http.HttpClient()).UpdateSiteAsync("C:/solution/src/Project/Sugcon/rendering/output", "78ee8e47-3d99-444b-99e9-a87e6dd5385b"));

            return context.Inputs;
        }
    }
}
