using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CryptologyLabs
{
    /// <summary>
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class LetterTable : Window
    {
        public LetterTable()
        {
            InitializeComponent();
        }

        public void PopulateData(Dictionary<char, int> data)
        {
            var letterPanel = (StackPanel)FindName("Letters");
            var amountPanel = (StackPanel)FindName("Amount");
            
            foreach(var item in data.OrderBy(v => v.Key))
            {
                letterPanel.Children.Add(new Label()
                {
                    Content = item.Key,
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush() { Color = Colors.Black },
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontSize = 18
                });

                amountPanel.Children.Add(new Label()
                {
                    Content = item.Value,
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush() { Color = Colors.Black },
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontSize = 18
                });
            }
        }
    }
}
