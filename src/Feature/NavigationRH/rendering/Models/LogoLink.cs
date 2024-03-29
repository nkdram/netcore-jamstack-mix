﻿using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response.Model.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Feature.Navigation.Rendering.Models
{
    public class LogoLink
    {
        public TextField NavigationTitle { get; set; }

        public ImageField HeaderLogo { get; set; }
    }
}
