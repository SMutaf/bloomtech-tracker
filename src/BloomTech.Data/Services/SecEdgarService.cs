using System.ServiceModel.Syndication;
using System.Xml;
using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;

namespace BloomTech.Data.Services
{
    public class SecEdgarService : IInsiderService
    {
        public async Task<List<InsiderTrade>> GetLatestTradesAsync(string symbol, string cik)
        {
            var trades = new List<InsiderTrade>();

            // SEC Atom Feed URL
            string url = $"https://www.sec.gov/cgi-bin/browse-edgar?action=getcompany&CIK={cik}&type=4&count=20&output=atom";

            try
            {
                var settings = new XmlReaderSettings { Async = true };

                using var client = new HttpClient();
                // Format hatasını aşmak için TryAddWithoutValidation 
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "BloomTechTracker mutafmutaf76@gmail.com");

                var stream = await client.GetStreamAsync(url);
                using var reader = XmlReader.Create(stream, settings);
                var feed = SyndicationFeed.Load(reader);

                foreach (var item in feed.Items)
                {
                    var transactionDate = item.LastUpdatedTime.DateTime;

                    if (transactionDate == DateTime.MinValue)
                        transactionDate = item.PublishDate.DateTime;
                    if (transactionDate == DateTime.MinValue)
                        transactionDate = DateTime.Now;

                    string type = "Form 4";
                    if (item.Title.Text.Contains("-"))
                    {
                        type = "Form " + item.Title.Text.Split('-')[0].Trim();
                    }


                    string filerName = $"{symbol} Insider";

                    trades.Add(new InsiderTrade
                    {
                        Name = filerName,
                        TransactionDate = transactionDate,
                        Type = type,
                        Shares = 0, // XML'de sayı yok
                        Amount = 0, // XML'de tutar yok
                        Url = item.Links.FirstOrDefault()?.Uri.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SEC Hatası: {ex.Message}");
            }

            return trades;
        }
    }
}