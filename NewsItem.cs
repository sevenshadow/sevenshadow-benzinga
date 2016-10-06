using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenShadow.Core.Entities.News
{
    public class NewsItem : INewsItem
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public string Headline { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime RetrieveDate { get; set; }
        public string URL { get; set; }
        public string Body { get; set; }
        public string Teaser { get; set; }
        public List<string> Tickers { get; set; }
        public string TickerListString { get; set; }
        public List<string> Channels { get; set; }
        public List<string> Tags { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
