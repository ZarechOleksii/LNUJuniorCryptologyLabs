using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace CryptologyLabs.RSA
{
    /// <summary>
    /// Interaction logic for RSAModal.xaml
    /// </summary>
    public partial class RSAModal : Window, ICryptable
    {
        private RSACryptoServiceProvider _RSA;
        private Encoding _encoder;

        public RSAModal()
        {
            InitializeComponent();
            _encoder = new UTF8Encoding();
        }

        public RSAModal(RSACryptoServiceProvider provider)
        {
            _RSA = provider;
            _encoder = new UTF8Encoding();
        }

        public string? Attack(string sourceText, string encryptedText)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string encryptedText)
        {
            var toEncode = Convert.FromBase64String(encryptedText);
            var decrypted = _RSA.Decrypt(toEncode, false);
            return _encoder.GetString(decrypted);
        }

        public string Encrypt(string sourceText)
        {
            var toEncode = _encoder.GetBytes(sourceText);
            var encrypted = _RSA.Encrypt(toEncode, false);
            return Convert.ToBase64String(encrypted);
        }

        private void NewKeys_Click(object sender, RoutedEventArgs e)
        {
            _RSA = new RSACryptoServiceProvider(16384);

            var dialog = new SaveFileDialog
            {
                Title = "Save notepad with keys:",
                FileName = $"CipherNotePad-{DateTime.Now:dd-MM-yyyy HH mm}",
                Filter = "All Files (*.*)|*.*"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                File.WriteAllBytes(dialog.FileName, _RSA.ExportCspBlob(true));
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }

            Close();
        }

        private void ExistingKeys_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choose key notepad:"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                var blob = File.ReadAllBytes(dialog.FileName);
                _RSA = new RSACryptoServiceProvider(16384);
                _RSA.ImportCspBlob(blob);
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }

            Close();
        }
    }
}
