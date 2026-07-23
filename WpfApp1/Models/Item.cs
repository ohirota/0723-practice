using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Item
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int MarketPrice { get; set; }

        public int Difference => Math.Abs(Price - MarketPrice);
        public string Judgement
        {
            get
            {
                if (Difference < 3000) return "買い";
                else if (Difference < 5000) return "見送り";
                else return "きつい";
            }
        }
    }
}
