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

namespace CryptologyLabs.Vigenere
{
    public partial class VigenereEncryptor : ICryptable
    {
        private string? _key;

        public VigenereEncryptor()
        {

        }

        public bool? ShowDialog()
        {
            return true;
        }

        public VigenereEncryptor(string key)
        {
            _key = key;
        }

        public string? Attack(string sourceText, string encryptedText)
        {
            var key = GetKey(sourceText, encryptedText);

            MessageBox.Show(key,
                "Key found: ", 
                MessageBoxButton.OK);
            return key;
        }
        public static string GetKey(string sourceText, string encryptedText)
        {
            var key = new string(
                sourceText
                .Zip(encryptedText)
                .Select(q =>
                {
                    if (Alphabets.IsEnglish(q.First))
                        return q.Second - q.First >= 0 ? (char)(q.Second - q.First + 'A') : (char)(q.Second - q.First + Alphabets.englishLen + 'A');
                    if (Alphabets.ukrainian.Contains(q.First))
                    {
                        var currentIndex = Alphabets.ukrainian.IndexOf(q.Second) - Alphabets.ukrainian.IndexOf(q.First);
                        currentIndex += currentIndex >= 0 ? 0 : Alphabets.ukrainianLen;
                        return Alphabets.ukrainianCapital[currentIndex];
                    }
                    if (Alphabets.ukrainianCapital.Contains(q.First))
                    {
                        var currentIndex = Alphabets.ukrainianCapital.IndexOf(q.Second) - Alphabets.ukrainianCapital.IndexOf(q.First);
                        currentIndex += currentIndex >= 0 ? 0 : Alphabets.ukrainianLen;
                        return Alphabets.ukrainianCapital[currentIndex];
                    }

                    return 'A';
                })
                .ToArray());

            return key;
        }

        public string Decrypt(string encryptedText)
        {
            if (_key is null)
                if (!SelectKeyNotepad())
                {
                    MessageBox.Show("No key :)",
                        "No key :)",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return string.Empty;
                }

            return new string(
                encryptedText
                .Zip(_key)
                .Select((zip, i) => DecryptChar(zip.First, zip.Second))
                .ToArray());
        }

        public string Encrypt(string sourceText)
        {
            if (_key is null)
                if (!GenerateNewKeyNotepad(sourceText))
                {
                    MessageBox.Show("No key :)",
                        "No key :)",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return string.Empty;
                }

            return new string(
                sourceText
                .Zip(_key)
                .Select((zip, i) => EncryptChar(zip.First, zip.Second))
                .ToArray());
        }

        private static char DecryptChar(char toDecrypt, char fromKey)
        {
            var toMove = ToMove(fromKey);

            if (Alphabets.IsCapitalEnglish(toDecrypt))
                return (char)((toDecrypt - toMove > 64) ? toDecrypt - toMove : toDecrypt - toMove + Alphabets.englishLen);

            if (Alphabets.IsSmallEnglish(toDecrypt))
                return (char)((toDecrypt - toMove > 96) ? toDecrypt - toMove : toDecrypt - toMove + Alphabets.englishLen);

            if (Alphabets.ukrainian.Contains(toDecrypt))
            {
                var genIndex = Alphabets.ukrainian.IndexOf(toDecrypt) - toMove;
                genIndex += genIndex < 0 ? Alphabets.ukrainianLen : 0;
                return Alphabets.ukrainian[(genIndex) % Alphabets.ukrainianLen];
            }    

            if (Alphabets.ukrainianCapital.Contains(toDecrypt))
            {
                var genIndex = Alphabets.ukrainianCapital.IndexOf(toDecrypt) - toMove;
                genIndex += genIndex < 0 ? Alphabets.ukrainianLen : 0;
                return Alphabets.ukrainianCapital[(genIndex) % Alphabets.ukrainianLen];
            }

            return toDecrypt;
        }

        private static char EncryptChar(char toEncrypt, char fromKey)
        {
            var toMove = ToMove(fromKey);

            if (Alphabets.IsCapitalEnglish(toEncrypt))
                return (char)((toEncrypt + toMove < 91) ? toEncrypt + toMove : toEncrypt + toMove - Alphabets.englishLen);

            if (Alphabets.IsSmallEnglish(toEncrypt))
                return (char)((toEncrypt + toMove < 123) ? toEncrypt + toMove : toEncrypt + toMove - Alphabets.englishLen);

            if (Alphabets.ukrainian.Contains(toEncrypt))
                return Alphabets.ukrainian[(Alphabets.ukrainian.IndexOf(toEncrypt) + toMove) % Alphabets.ukrainianLen];

            if (Alphabets.ukrainianCapital.Contains(toEncrypt))
                return Alphabets.ukrainianCapital[(Alphabets.ukrainianCapital.IndexOf(toEncrypt) + toMove) % Alphabets.ukrainianLen];

            return toEncrypt;
        }

        private static int ToMove(char letter)
        {
            if (Alphabets.IsCapitalEnglish(letter))
                return letter - 'A';

            if (Alphabets.IsSmallEnglish(letter))
                return letter - 'a';

            if (Alphabets.ukrainian.Contains(letter))
                return Alphabets.ukrainian.IndexOf(letter);

            if (Alphabets.ukrainianCapital.Contains(letter))
                return Alphabets.ukrainianCapital.IndexOf(letter);

            return 0;
        }

        private bool SelectKeyNotepad()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choose key notepad:"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                using var stream = dialog.OpenFile();
                using var reader = new StreamReader(stream);
                _key = reader.ReadToEnd();
                return true;
            }

            return false;
        }

        private bool GenerateNewKeyNotepad(string source)
        {
            Random random = new();

            var toWrite = new string(
                source
                .AsParallel()
                .AsOrdered()
                .Select(q =>
                {
                    if (Alphabets.IsEnglish(q))
                        return (char)(random.Next(Alphabets.englishLen) + 'A');
                    if (Alphabets.IsUkrainian(q))
                        return Alphabets.ukrainianCapital[random.Next(Alphabets.ukrainianLen)];

                    return 'A';
                })
                .ToArray());

            var dialog = new SaveFileDialog
            {
                Title = "Save notepad with key:",
                FileName = $"CipherNotePad-{DateTime.Now:dd-MM-yyyy HH mm}",
                Filter = "All Files (*.*)|*.*"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                using var stream = dialog.OpenFile();
                using var writer = new StreamWriter(stream);
                writer.Write(toWrite);
                _key = toWrite;
                return true;
            }

            return false;
        }
    }
}
