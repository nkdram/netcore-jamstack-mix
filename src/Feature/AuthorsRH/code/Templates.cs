using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sugcon.Feature.AuthorsRH
{
    public static class Templates
    {
        public static class Authors
        {
            public static readonly ID Id = new ID("{8A16D41F-EE3F-4EA2-93CA-33F2614BD724}");

            public static class Fields
            {
                public static readonly ID Name = new ID("{096FFE16-5F8F-47C7-8C88-44A9F2855A2E}");
                public static readonly ID DisplayPicture = new ID("{ABEB2414-0C92-437C-9E67-23B7ABCA0C06}");
                public static readonly ID ShortNote = new ID("{C378888D-4A38-49BB-AEEC-9F72132E0A15}");
                public static readonly ID Description = new ID("{0BFFA33A-9731-4093-AEA9-1904618988F8}");
            }
        }
    }
}