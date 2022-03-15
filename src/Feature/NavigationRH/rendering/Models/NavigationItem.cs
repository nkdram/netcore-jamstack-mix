using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Feature.Navigation.Rendering.Models
{
    public class NavigationItem
    {
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
    }
}
