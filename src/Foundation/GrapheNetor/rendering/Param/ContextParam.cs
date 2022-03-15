using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Param
{
    /// <summary>
    /// Context Param used in Graphql querying
    /// </summary>
    public class ContextParam
    {
        /// <summary>
        /// Site name : ex : examplesite
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Provide Sitecore language ex: en
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Sitecore page path or route path ex: /Articles/Some random article page
        /// </summary>
        public string RoutePath { get; set; }
    }
}
