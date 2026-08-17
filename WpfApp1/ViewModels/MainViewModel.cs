using CommunityToolkit.Mvvm.ComponentModel;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using WpfApp1.Models;


namespace WpfApp1.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private List<Item> _allLines = new List<Item>()
        {
            new Item{ Name = "アクセA", Price = 1000,  MarketPrice = 900,   Gram = 1.23, MaterialType = Material.K18 },
            new Item{ Name = "アクセB", Price = 33500, MarketPrice = 37500, Gram = 4.5,  MaterialType = Material.Pt850 },
            new Item{ Name = "アクセC", Price = 10000, MarketPrice = 90000, Gram = 6.7,  MaterialType = Material.K18 },
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

            // ① 探す
            var Gold750 = doc.DocumentNode.SelectSingleNode("//th[contains(text(), 'K18 (純度 75％)')]/following-sibling::td");
            var Pla850 = doc.DocumentNode.SelectSingleNode("//th[contains(text(), 'Pt 850 (純度 85％)')]/following-sibling::td");

            // ② 文字列にする（メソッドに任せる）
            string priceGold = CaseNullHandling(Gold750);
            string pricePla = CaseNullHandling(Pla850);

            // ③ 画面に渡す
            Market.GoldPrice = priceGold;
            Market.PlaPrice = pricePla;

            // ④ 相場を数値にして、各商品の相場額を計算する
            int goldPerGram = ParsePrice(priceGold);
            int plaPerGram = ParsePrice(pricePla);

            foreach (Item item in FilteredLines)
            {
                int perGram;
                if (item.MaterialType == Material.K18)
                {
                    perGram = goldPerGram;
                }
                else
                {
                    perGram = plaPerGram;
                }

                // 1グラム単価 × 重さ（小数は切り捨て）
                item.MarketPrice = (int)(perGram * item.Gram);
            }

        }

        private string CaseNullHandling(HtmlNode node)
        {
            if (node == null)
            {
                return "反映できない";
            }

            else
            {
                return HtmlEntity.DeEntitize(node.InnerText);
            }
        }

        // 文字列を数字に変更する処理
        // 変換できない場合（「反映できない」など）は 0 を返す
        private int ParsePrice(string text)
        {
            string s = text.Replace("¥", "").Replace(" ", "").Replace(",", "");

            if (int.TryParse(s, out int price))
            {
                return price;
            }
            else
            {
                return 0;
            }
        }

    }
}
