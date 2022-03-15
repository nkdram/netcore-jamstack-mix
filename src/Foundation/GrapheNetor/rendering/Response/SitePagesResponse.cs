using Newtonsoft.Json;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Response
{
    public class SitePagesResponse : IGraphQlResponse
    {

        [JsonProperty(PropertyName = "layout")]
        public Layout layout { get; set; }

        public class Layout
        {
            [JsonProperty(PropertyName = "item")]
            public Item item { get; set; }
        }

        public class Item
        {
            public string id { get; set; }
            public string path { get; set; }
            public string homeItemPath { get; set; }
            public Language language { get; set; }
            public Children children { get; set; }
        }

        public class Language
        {
            public string name { get; set; }
        }

        public class Children
        {
            public Item[] results { get; set; }
        }

    }
}
