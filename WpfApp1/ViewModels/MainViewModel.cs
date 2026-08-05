using CommunityToolkit.Mvvm.ComponentModel;
using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WpfApp1.Models;


namespace WpfApp1.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private List<Item> _allLines = new List<Item>()
        {
            new Item{ Name = "アクセA", Price =1000, MarketPrice = 900},
            new Item{ Name = "アクセB", Price =33500, MarketPrice = 37500},
            new Item{ Name = "アクセC", Price =10000, MarketPrice = 90000}
        };

        public ObservableCollection<Item> FilteredLines { get; set; }
        public MarketPrice Market { get; set; } = new MarketPrice();

        public MainViewModel()
        {
            FilteredLines = new ObservableCollection<Item>(_allLines);
            _ = LoadMarketDataAsync();   // ここで呼び出す
        }

        private async Task LoadMarketDataAsync()
        {
            string filePath = "nexus_cache.html";
            string html;

            if (System.IO.File.Exists(filePath))
            {
                // ファイルがあれば、そこから読み込む（外部アクセスしない）
                html = System.IO.File.ReadAllText(filePath);
            }
            else
            {
                // ファイルがなければ、サイトからとって保存する
                string url = "https://www.nexus13.co.jp/";
                using (HttpClient client = new HttpClient())
                {
                    html = await client.GetStringAsync(url);
                }
                System.IO.File.WriteAllText(filePath, html);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var Gold750 = doc.DocumentNode.SelectSingleNode("//th[contains(text(), 'K18 (純度 75％)')]/following-sibling::td");

            // 取得できた場合とできなかった場合で入れる値が変わるため、先に宣言だけしておく
            string priceGold;

            // サイト側の表記が変わると SelectSingleNode が null を返すため、先に確認する
            if (Gold750 == null)
            {
                priceGold = "反映できない";
            }
            else
            {
                priceGold = HtmlEntity.DeEntitize(Gold750.InnerText);
            }

            var Pla850 = doc.DocumentNode.SelectSingleNode("//th[contains(text(), 'Pt 850 (純度 85％)')]/following-sibling::td");
            string pricePla;

            if (Pla850 == null)
            {
                pricePla = "反映できない";
            }
            else
            {
                pricePla = HtmlEntity.DeEntitize(Pla850.InnerText);
            }

            Market.GoldPrice = priceGold;
            Market.PlaPrice = pricePla;
        }


    }
}
