using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfApp1.Models
{
    public class MarketPrice : ObservableObject
    {
        private string _k24Price;
        public string K24Price
        {
            get => _k24Price;
            set => SetProperty(ref _k24Price, value);
        }

        private string _k18Price;
        public string K18Price
        {
            get => _k18Price;
            set => SetProperty(ref _k18Price, value);
        }

        private string _pt900Price;
        public string Pt900Price
        {
            get => _pt900Price;
            set => SetProperty(ref _pt900Price, value);
        }

        private string _pt850Price;
        public string Pt850Price
        {
            get => _pt850Price;
            set => SetProperty(ref _pt850Price, value);
        }
    }
}
