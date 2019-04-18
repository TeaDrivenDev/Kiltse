using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeaDriven.Kiltse.Demo
{
    public partial class MainWindow
    {
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

        public MainWindow()
        {
            InitializeComponent();

            Items.Add(
                new Item()
                {
                    Left = 100,
                    Top = 100,
                    Name = "Grmpf",
                    SubItems = new List<string>() { "Grah", "Narf", "Grr", "Aaaaaaaaah", "Gargh" }
                });
        }
    }

    public class Item
    {
        public double Left { get; set; }

        public double Top { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> SubItems { get; set; }
    }
}
