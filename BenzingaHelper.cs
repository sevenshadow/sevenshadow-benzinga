using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

using SevenShadow.Core.Entities.News;

namespace SevenShadow.Benzinga
{
    public class BenzingaHelper
    {
        private const string BENZINGA_NEWS_API_URL = "http://api.benzinga.com/api/v2/";

        private string AuthToken;

        public BenzingaHelper(string authenticationToken = "")
        {
            AuthToken = authenticationToken;
        }

        public void SetAuthToken(string token)
        {
            AuthToken = token;
        }

        public string GetRawData(string dataset, IDictionary<string, string> settings, string format = "json")
        {
            /*  This is just to get the ticks to send to benzing.
             * 
             * 
            double javascriptTicks = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalMilliseconds;
            long javascriptTicksLong = (long)javascriptTicks;
            javascriptTicksLong = javascriptTicksLong;
            
             */
            string requestUrl = "";
            string rawData = "";
            var requestData = new StringBuilder(settings.Count());

            if (string.IsNullOrEmpty(AuthToken))
            {
                throw new Exception("Benzinga Token Not Set.");
            }
            else
            {
                requestUrl = BENZINGA_NEWS_API_URL + String.Format("{0}/?token={1}", dataset, AuthToken);
            }

            foreach (KeyValuePair<string, string> kvp in settings)
            {
                requestData.Append(String.Format("&{0}={1}", kvp.Key, kvp.Value));
            }
            requestUrl = requestUrl + requestData.ToString();
            try
            {
                //Prevent 404 Errors:
                WebClient client = new WebClient();
                if (format == "json")
                    client.Headers.Add("Accept: application/json");

                rawData = client.DownloadString(requestUrl);
            }
            catch (Exception err)
            {
                throw new Exception("Sorry there was an error and we could not connect to Benzinga: " + err.Message);
            }

            return rawData;
        }

        public List<NewsItem> GetNews()
        {
            string rawData = GetRawData("news", new Dictionary<string, string>());

            List<BenzingaNewsItem> benzingaNewsItems = (List<BenzingaNewsItem>)JsonConvert.DeserializeObject(rawData, typeof(List<BenzingaNewsItem>));
            List<NewsItem> translatedItems = TranslateNewsItems(benzingaNewsItems);
            return translatedItems;

        }

        public List<NewsItem> GetNewsByDate(DateTime fromDate, bool includeStoryText = false)
        {
            Dictionary<string, string> settings = new Dictionary<string,string>();
            settings.Add("publishedSince", ConvertToTimestamp(fromDate).ToString());
            settings.Add("pageSize", "100");
            settings.Add("date", fromDate.ToString("yyyy-MM-dd"));
            if (includeStoryText)
                settings.Add("displayOutput", "full");
            string rawData = GetRawData("news", settings);

            List<BenzingaNewsItem> benzingaNewsItems = (List<BenzingaNewsItem>)JsonConvert.DeserializeObject(rawData, typeof(List<BenzingaNewsItem>));
            List<NewsItem> translatedItems = TranslateNewsItems(benzingaNewsItems);
            return translatedItems;

        }

        private double ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (double)span.TotalSeconds;
        }
        public List<NewsItem> GetNews(List<string> tickers)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("tickers", string.Join(",", tickers));

            string rawData = GetRawData("news", dic);

            List<BenzingaNewsItem> benzingaNewsItems = (List<BenzingaNewsItem>)JsonConvert.DeserializeObject(rawData, typeof(List<BenzingaNewsItem>));
            List<NewsItem> translatedItems = TranslateNewsItems(benzingaNewsItems);
            
            return translatedItems;

        }


        #region Private Methods

        private List<NewsItem> TranslateNewsItems(List<BenzingaNewsItem> items)
        {
            List<NewsItem> translatedItems = (from x in items
                                              select new NewsItem()
                                              {
                                                  ExternalId = x.id.ToString(),
                                                  Headline = x.title,
                                                  Author = x.author,
                                                  URL = x.url,
                                                  Teaser = x.teaser,
                                                  Body = x.body,
                                                  Tickers = (from y in x.stocks
                                                             select y.name).ToList(),
                                                  Tags = (from y in x.tags
                                                          select y.name).ToList(),
                                                  Channels = (from y in x.stocks
                                                              select y.name).ToList(),
                                                  TickerListString = string.Join("|", (from y in x.stocks
                                                                                       select y.name).ToArray()),
                                                  DateCreated = DateTime.Parse(x.created),
                                                  DateModified = DateTime.Parse(x.updated)

                                              }

                                 ).ToList();

            return translatedItems;
        }

        #endregion

    }
}
