using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CryptologyLabs.Gamma
{
    public partial class GammaEncryptor : ICryptable
    {
        private string? _gamma;

        public GammaEncryptor()
        {

        }

        public bool? ShowDialog()
        {
            return true;
        }

        public GammaEncryptor(string gamma)
        {
            _gamma = gamma;
        }

        public string? Attack(string sourceText, string encryptedText)
        {
            var gamma = GetGamma(sourceText, encryptedText);

            MessageBox.Show(gamma,
                "Gamma found: ", 
                MessageBoxButton.OK);
            return gamma;
        }

        public static string? GetGamma(string sourceText, string encryptedText)
        {
            return new string(
                sourceText
                .Zip(encryptedText)
                .Select(q =>
                    AttackChar(q.First, q.Second))
                .ToArray());
        }

        public string Decrypt(string encryptedText)
        {
            if (_gamma is null)
                if (!SelectGammaNotepad())
                {
                    MessageBox.Show("No gamma :)",
                        "No gamma :)",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return string.Empty;
                }

            return new string(
                encryptedText
                .Zip(_gamma)
                .Select((zip, i) => DecryptChar(zip.First, zip.Second))
                .ToArray());
        }

        public string Encrypt(string sourceText)
        {
            if (_gamma is null)
                if (!GenerateNewGammaNotepad(sourceText))
                {
                    MessageBox.Show("No gamma :)",
                        "No gamma :)",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return string.Empty;
                }

            return new string(
                sourceText
                .Zip(_gamma)
                .Select((zip, i) => EncryptChar(zip.First, zip.Second))
                .ToArray());
        }

        private static char AttackChar(char source, char encrypted)
        {
            var result = source ^ encrypted;
            return (char)result;
        }

        private static char DecryptChar(char toDecrypt, char fromGamma)
        {
            var result = toDecrypt ^ fromGamma;
            return (char)result;
        }

        private static char EncryptChar(char toEncrypt, char fromGamma)
        {
            var result = toEncrypt ^ fromGamma;
            return (char)result;
        }

        private bool SelectGammaNotepad()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choose gamma notepad:"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                using var stream = dialog.OpenFile();
                using var reader = new StreamReader(stream);
                _gamma = reader.ReadToEnd();
                return true;
            }

            return false;
        }

        private bool GenerateNewGammaNotepad(string source)
        {
            Random random = new();

            var toWrite = new string(
                source
                .AsParallel()
                .AsOrdered()
                .Select(q =>
                {
                    return (char)random.Next(2048);
                })
                .ToArray());

            var dialog = new SaveFileDialog
            {
                Title = "Save notepad with gamma:",
                FileName = $"CipherNotePad-{DateTime.Now:dd-MM-yyyy HH mm}",
                Filter = "All Files (*.*)|*.*"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                using var stream = dialog.OpenFile();
                using var writer = new StreamWriter(stream);
                writer.Write(toWrite);
                _gamma = toWrite;
                return true;
            }

            return false;
        }
    }
}
