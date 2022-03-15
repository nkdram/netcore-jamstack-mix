using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Sugcon.Feature.ProductsRH
{
  public static class Templates
  {
        public static class Product
        {
            public static readonly ID Id = new ID("{388d492b-8410-48e8-8acc-34e358fbbb78}");

            public static class Fields
            {
                public static readonly ID Title = new ID("{c52c9ca9-62d4-4595-bfda-c14a7d0fe0dd}");
                public static readonly ID ShortDescription = new ID("{c6ed7eeb-a723-4cb7-babe-0b4ba8f6b7fc}");
                public static readonly ID Image = new ID("{cf28ce48-6860-46af-a734-520c59f37208}");
                public static readonly ID Features = new ID("{5fcb513d-0042-4c97-8de8-a9b659da283b}");
                public static readonly ID Price = new ID("{190bfbc8-3391-4189-b33b-7e3098eff7bf}");
                public static readonly ID RelatedProducts = new ID("{b2b4822d-efd5-4de7-ad29-c48fa97b2df2}");
            }
        }
  }
}