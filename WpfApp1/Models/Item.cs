using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfApp1.Models
{
    public class Item : ObservableObject
    {
        public string Name { get; set; }
        public int Price { get; set; }
        private int _marketPrice;
        public int MarketPrice
        {
            get => _marketPrice;
            set
            {
                if (SetProperty(ref _marketPrice, value))
                {
                    OnPropertyChanged(nameof(OverPayment));
                    OnPropertyChanged(nameof(Judgement));
                }
            }
        }

        public double Gram { get; set; }
        public Material MaterialType { get; set; }

        public int OverPayment => Price - MarketPrice;    //相場より多く払う金額
        public string Judgement
        {
            get
            {
                if (OverPayment < 0) return "超買い";
                else if (OverPayment < 3000) return "買い";
                else if (OverPayment < 5000) return "見送り";
                else return "きつい";
            }
        }
    }
}
