using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering
{
    /// <summary>
    /// constant store
    /// </summary>
    public static class Constants
    {
        public const string Generator_Module_PagesResponseKey = "SC-Process-Page-Key";

        public const string Generator_Module_LayoutResponseKey = "SC-Layout-Page-Key";

        public const string Generator_Module_SitecoreItemKey = "SC-Item-Key";

        public const string Generator_Module_SitecoreImageKey = "SC-Image-Key";

        #region Netify Configuration Constants
        public const string Configuration_GrapheNetor_Netlify_Token = "Foundation:GrapheNetor:Netlify:Token";

        public const string Configuration_GrapheNetor_Netlify_SourcePath = "Foundation:GrapheNetor:Netlify:SourcePath";

        public const string Configuration_GrapheNetor_Netlify_SiteId = "Foundation:GrapheNetor:Netlify:SiteId";
        #endregion

        #region Pipeline Names
        public const string Page_Pipeline = "Pages Pipeline";

        public const string CopyAsset_Pipeline = "Copy Assets";
        #endregion

        #region Default Site 
        public const string Configuration_GrapheNetor_Pipelines_Language = "Foundation:GrapheNetor:Pipelines:Language";

        public const string Configuration_GrapheNetor_Pipelines_Site = "Foundation:GrapheNetor:Pipelines:Site";

        public const string Configuration_GrapheNetor_Pipelines_Path = "Foundation:GrapheNetor:Pipelines:Path";
        #endregion

    }
}
