using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenShadow.Benzinga
{
    public class BenzingaNewsItem
    {
        public int id { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string teaser { get; set; }
        public string url { get; set; }
        public string body { get; set; }
        public List<MetaDataItem> channels { get; set; }
        public List<MetaDataItem> tags { get; set; }
        public List<MetaDataItem> stocks { get; set; }
        // public string image { get; set; }
       
        public string created { get; set; }
        public string updated { get; set; }

    }

    public class MetaDataItem
    {
        public string name { get; set; }
    }
}
