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
    /// Interaction logic for TrithemiusModal.xaml
    /// </summary>
    public partial class TrithemiusModal : Window, ICryptable
    {
        private BaseTrithemiusEncryption? _encryptor;

        private string LinearA { get => ((TextBox)FindName("LinearABox")).Text; }

        private string LinearB { get => ((TextBox)FindName("LinearBBox")).Text; }

        private string NonLinearA { get => ((TextBox)FindName("NonLinearABox")).Text; }

        private string NonLinearB { get => ((TextBox)FindName("NonLinearBBox")).Text; }

        private string NonLinearC { get => ((TextBox)FindName("NonLinearCBox")).Text; }

        private string Watchword { get => ((TextBox)FindName("WatchwordBox")).Text; }

        public TrithemiusModal()
        {
            InitializeComponent();
        }

        public string Encrypt(string sourceText)
        {
            if (_encryptor != null)
                return _encryptor.Encrypt(sourceText);

            throw new Exception("How?");
        }

        public string Decrypt(string encryptedText)
        {
            if (_encryptor != null)
                return _encryptor.Decrypt(encryptedText);

            throw new Exception("How?");
        }

        public string? Attack(string sourceText, string encryptedText)
        {
            TrithemiusMethod modal = new();
            modal.ShowDialog();

            if (modal.Encryptor != null)
            {
                return modal.Encryptor.Attack(sourceText, encryptedText);
            }

            return null;
        }

        private void Linear_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(LinearA, out int parA) 
                && int.TryParse(LinearB, out int parB))
            {
                DialogResult = true;
                _encryptor = new LinearEncryption(parA, parB);
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Please provide integers for input",
                    "Wrong input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void NonLinear_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NonLinearA, out int parA) 
                && int.TryParse(NonLinearB, out int parB) 
                && int.TryParse(NonLinearC, out int parC))
            {
                DialogResult = true;
                _encryptor = new NonLinearEncryption(parA, parB, parC);
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Please provide integers for input",
                    "Wrong input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Watchword_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Watchword))
            {
                DialogResult = true;
                _encryptor = new WatchwordEncryption(Watchword);
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Please provide a watchword",
                    "Wrong input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
