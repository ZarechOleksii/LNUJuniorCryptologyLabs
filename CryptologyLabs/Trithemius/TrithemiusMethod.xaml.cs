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

namespace CryptologyLabs.Trithemius
{
    /// <summary>
    /// Interaction logic for TrithemiusMethod.xaml
    /// </summary>
    public partial class TrithemiusMethod : Window
    {
        public BaseTrithemiusEncryption? Encryptor;

        public TrithemiusMethod()
        {
            InitializeComponent();
        }

        private void Linear_Attack_Click(object sender, RoutedEventArgs e)
        {
            Encryptor = new LinearEncryption();
            Close();
        }

        private void NonLinear_Attack_Click(object sender, RoutedEventArgs e)
        {
            Encryptor = new NonLinearEncryption();
            Close();
        }

        private void Watchword_Attack_Click(object sender, RoutedEventArgs e)
        {
            Encryptor = new WatchwordEncryption();
            Close();
        }
    }
}
