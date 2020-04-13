using System;
using System.Collections.Generic;

namespace NASAImageGalleryREST
{
    /// <summary>
    /// Represents the structure of a JSON response from the NASA Gallery API.
    /// </summary>
    public class NasaRepository
    {
        public class Link
        {
            public string rel { get; set; }
            public string href { get; set; }
            public string render { get; set; }
        }

        public class Datum
        {
            public string title { get; set; }
            public string center { get; set; }
            public string nasa_id { get; set; }
            public DateTime date_created { get; set; }
            public List<string> keywords { get; set; }
            public string description { get; set; }
            public string media_type { get; set; }
            public string description_508 { get; set; }
            public string secondary_creator { get; set; }
            public string location { get; set; }
            public string photographer { get; set; }
            public List<string> album { get; set; }
        }

        public class Item
        {
            public List<Link> links { get; set; }
            public string href { get; set; }
            public List<Datum> data { get; set; }
        }

        public class Link2
        {
            public string rel { get; set; }
            public string href { get; set; }
            public string prompt { get; set; }
        }

        public class Metadata
        {
            public int total_hits { get; set; }
        }

        public class Collection
        {
            public List<Item> items { get; set; }
            public List<Link2> links { get; set; }
            public string version { get; set; }
            public string href { get; set; }
            public Metadata metadata { get; set; }
        }

        public Collection collection { get; set; }
    }
}
