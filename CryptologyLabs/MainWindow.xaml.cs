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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Windows.Xps.Packaging;
using System.Printing;
using System.Diagnostics;

namespace CryptologyLabs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? _filePath;

        private string FileContent 
        { 
            get => ((TextBox)FindName("FromTextBox")).Text; 
            set => ((TextBox)FindName("FromTextBox")).Text = value; 
        }

        private string ResultContent
        {
            get => ((TextBox)FindName("ToTextBox")).Text;
            set => ((TextBox)FindName("ToTextBox")).Text = value;
        }

        public MainWindow()
        {
            InitializeComponent();
            FileContent = "";
            ResultContent = "";
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Create New File: ",
                DefaultExt = ".txt",
                Filter = "All Files (*.*)|*.*"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                using var stream = dialog.OpenFile();
                _filePath = dialog.FileName;
                Unlock_Items();
            }

        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Open document to cipher/decipher:"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                _filePath = dialog.FileName;
                if (IsBinary())
                {
                    var bytes = File.ReadAllBytes(dialog.FileName);
                    FileContent = Convert.ToBase64String(bytes);
                }
                else
                {
                    using var stream = dialog.OpenFile();
                    using var reader = new StreamReader(stream);
                    FileContent = reader.ReadToEnd();
                }

                Unlock_Items();
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save content from the first text box:",
                FileName = System.IO.Path.GetFileNameWithoutExtension(_filePath) + "",
                DefaultExt = System.IO.Path.GetExtension(_filePath),
                Filter = "All Files (*.*)|*.*"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                if (IsBinary())
                {
                    Byte[] bytes = Convert.FromBase64String(FileContent);
                    File.WriteAllBytes(dialog.FileName, bytes);
                }
                else
                {
                    using var stream = dialog.OpenFile();
                    using var writer = new StreamWriter(stream);
                    writer.Write(FileContent);
                }
            }
        }

        private void SaveFileResult_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save content from the second text box:",
                FileName = System.IO.Path.GetFileNameWithoutExtension(_filePath) + "Encrypted",
                DefaultExt = System.IO.Path.GetExtension(_filePath),
                Filter = "All Files (*.*)|*.*"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                if (IsBinary())
                {
                    Byte[] bytes = Convert.FromBase64String(ResultContent);
                    File.WriteAllBytes(dialog.FileName, bytes);
                }
                else
                {
                    using var stream = dialog.OpenFile();
                    using var writer = new StreamWriter(stream);
                    writer.Write(ResultContent);
                }
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo info = new()
            {
                UseShellExecute = true
            };

            info.Verb = "print";
            info.FileName = _filePath;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new();
            p.StartInfo = info;
            p.Start();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AboutDialog_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "By Oleksii Zarechanskyi 2022 \nGlory to Ukraine!", 
                "About", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            var modal = GetCryptable();
            var result = modal.ShowDialog();
            
            if (result == true)
            {
                ResultContent = modal.Encrypt(FileContent);
            }
            ((MenuItem)FindName("SaveResultFile")).IsEnabled = true;
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            var modal = GetCryptable();
            var result = modal.ShowDialog();

            if (result == true)
            {
                ResultContent = modal.Decrypt(FileContent);
            }
            ((MenuItem)FindName("SaveResultFile")).IsEnabled = true;
        }
        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            var modal = GetCryptable();

            if (string.IsNullOrEmpty(FileContent) || string.IsNullOrEmpty(ResultContent))
                MessageBox.Show(
                    "Boxes have to be not empty",
                    "Wrong input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

            modal.Attack(FileContent, ResultContent);
        }

        private ICryptable GetCryptable()
        {
            var selectedText = ((ComboBox)FindName("CipherMethodList")).SelectedValue.ToString();

            ICryptable cryptingModal = selectedText switch
            {
                "Ceasar" => new Ceasar.CeasarModal(),
                "Trithemius" => new Trithemius.TrithemiusModal(),
                "Gamma" => new Gamma.GammaEncryptor(),
                "Vigenere" => new Vigenere.VigenereEncryptor(),
                "RSA" => new RSA.RSAModal(),
                _ => throw new Exception(),
            };

            return cryptingModal;
        }

        private void Unlock_Items()
        {
            ((MenuItem)FindName("PrintFile")).IsEnabled = true;
            ((MenuItem)FindName("SaveFile")).IsEnabled = true;
            ((TextBox)FindName("FromTextBox")).IsReadOnly = false;
            ((Button)FindName("AttackButton")).IsEnabled = true;
            ((Button)FindName("EncryptButton")).IsEnabled = true;
            ((Button)FindName("DecryptButton")).IsEnabled = true;
        }

        private bool IsBinary()
        {
            if (System.IO.Path.GetExtension(_filePath) == ".jpg"
                    || System.IO.Path.GetExtension(_filePath) == ".jpeg"
                    || System.IO.Path.GetExtension(_filePath) == ".png")
                return true;
            return false;
        }
    }
}
