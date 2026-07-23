using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
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

        public MainViewModel()
        {
            FilteredLines = new ObservableCollection<Item>(_allLines);


            
        }
    }
}
