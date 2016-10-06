using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenShadow.Core.Entities.News
{
    public interface INewsItem
    {
        int Id { get; set; }
        string Author { get; set; }
        string Source { get; set; }
        string Headline { get; set; }
        DateTime PublishDate { get; set; }
        DateTime RetrieveDate { get; set; }
        string URL { get; set; }
        string Body { get; set; }
        string Teaser { get; set; }
        string TickerListString { get; set; }
        List<string> Tickers { get; set; }
        List<string> Channels { get; set; }
        List<string> Tags { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }

    }

    
}
