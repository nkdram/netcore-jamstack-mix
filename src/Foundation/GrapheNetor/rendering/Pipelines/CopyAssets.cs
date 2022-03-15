using Statiq.Common;
using Statiq.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Pipelines
{
     public class CopyAssetsPipeline : Pipeline
    {
        public CopyAssetsPipeline()
        {
            InputModules = new ModuleList
            {
                new CopyFiles("./{css,fonts,js,img}/**/*", "./*.{png,ico,webmanifest}")
            };
        }
    }
}
