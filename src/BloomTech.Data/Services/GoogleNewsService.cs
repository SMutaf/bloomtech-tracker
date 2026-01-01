using System.ServiceModel.Syndication;
using System.Xml;
using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;

namespace BloomTech.Data.Services
{
    public class GoogleNewsService : INewsService
    {
        public async Task<List<News>> GetLatestNewsAsync(string symbol)
        {
            var newsList = new List<News>();

            string rssUrl = $"https://news.google.com/rss/search?q={symbol}+stock&hl=en-US&gl=US&ceid=US:en";

            try
            {
                using var reader = XmlReader.Create(rssUrl);
                var feed = SyndicationFeed.Load(reader);

                foreach (var item in feed.Items)
                {
                    newsList.Add(new News
                    {
                        Title = item.Title.Text,
                        Source = item.SourceFeed?.Title?.Text ?? "Google News",
                        Url = item.Links.FirstOrDefault()?.Uri.ToString(),
                        PublishedDate = item.PublishDate.DateTime,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RSS Okuma Hatası: {ex.Message}");
            }

            return newsList;
        }
    }
}