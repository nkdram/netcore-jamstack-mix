using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Sugcon.Feature.NavigationRH
{
  public static class Templates
  {
        public static class NavigationItem
        {
            public static readonly ID Id = new ID("{ce6c3fb6-4e5c-40d6-b50b-8cfe0d6fb60e}");
            public static class Fields
            {
                public static readonly ID NavigationTitle = new ID("{5e14017e-68e5-4362-b0fb-7552528a1d41}");
            }
        }

        public static class NavigationRoot
        {
            public static readonly ID Id = new ID("{26f208f4-794e-4e15-83a7-be1f5258451e}");

            public static class Fields
            {
                public static readonly ID HeaderLogo = new ID("{830194df-bd3d-477c-ab2f-9e1eea78534a}");
                public static readonly ID FooterCopyright = new ID("{27b1a61c-57d1-4159-8fce-a22b9576b514}");
            }
        }
  }
}