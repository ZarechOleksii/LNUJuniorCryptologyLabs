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

namespace CryptologyLabs.Ceasar
{
    /// <summary>
    /// Interaction logic for CeasarModal.xaml
    /// </summary>
    public partial class CeasarModal : Window, ICryptable
    {
        private int _move = 0;

        private Dictionary<char, int> _encounters;

        private string MoveBoxContent
        {
            get => ((TextBox)FindName("MoveBox")).Text;
        }

        public CeasarModal()
        {
            InitializeComponent();
            _encounters = new();
        }

        public string Encrypt(string sourceText)
        {
            _encounters = new();

            var toReturn = new string(sourceText.Select(v => EncodeChar(v)).ToArray());

            var letterTable = new LetterTable();

            letterTable.PopulateData(_encounters);

            letterTable.Show();

            return toReturn;
        }

        public string Decrypt(string encryptedText)
        {
            return new string(encryptedText.Select(v => DecodeChar(v)).ToArray());
        }

        public string? Attack(string sourceText, string encryptedText)
        {
            _move = 0;

            while(_move < 10000)
            {
                if (Decrypt(encryptedText) == sourceText)
                {
                    MessageBox.Show(
                        $"Key for ceasar cipher: {_move}",
                        "Attack Result",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return _move.ToString();
                }

                _move++;
            }
            if (_move == 1000)
                MessageBox.Show(
                       "Brute force attack failed, text from second box should be encrypted version of the first box in order for it to work.",
                       "Wrong input",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
            return null;
        }

        private char EncodeChar(char from)
        {
            if (Alphabets.ukrainian.Contains(from))
            {
                var current = Alphabets.ukrainian.IndexOf(from);

                if (_encounters.ContainsKey(Alphabets.ukrainianCapital[current]))
                    _encounters[Alphabets.ukrainianCapital[current]]++;
                else
                    _encounters[Alphabets.ukrainianCapital[current]] = 1;

                int index = (current + _move) % Alphabets.ukrainianLen;

                if (index < 0)
                    index += Alphabets.ukrainianLen;

                return Alphabets.ukrainian[index];
            }

            if (Alphabets.ukrainianCapital.Contains(from))
            {
                var current = Alphabets.ukrainianCapital.IndexOf(from);

                if (_encounters.ContainsKey(from))
                    _encounters[from]++;
                else
                    _encounters[from] = 1;

                int index = (current + _move) % Alphabets.ukrainianLen;

                if (index < 0)
                    index += Alphabets.ukrainianLen;

                return Alphabets.ukrainianCapital[index];
            }

            if (from > 64 && from < 91)
            {
                int current = from - 'A';

                if (_encounters.ContainsKey(from))
                    _encounters[from]++;
                else
                    _encounters[from] = 1;

                int index = (current + _move) % Alphabets.englishLen;

                if (index < 0)
                    index += Alphabets.englishLen;

                return (char)(index + 'A');
            }

            if (from > 96 && from < 123)
            {
                int current = from - 'a';

                if (_encounters.ContainsKey((char)(current + 'A')))
                    _encounters[(char)(current + 'A')]++;
                else
                    _encounters[(char)(current + 'A')] = 1;

                int index = (current + _move) % Alphabets.englishLen;

                if (index < 0)
                    index += Alphabets.englishLen;

                return (char)(index + 'a');
            }

            return from;
        }

        private char DecodeChar(char from)
        {
            if (Alphabets.ukrainian.Contains(from))
            {
                var current = Alphabets.ukrainian.IndexOf(from);

                int index = (current - _move) % Alphabets.ukrainianLen;

                if (index < 0)
                    index += Alphabets.ukrainianLen;

                return Alphabets.ukrainian[index];
            }

            if (Alphabets.ukrainianCapital.Contains(from))
            {
                var current = Alphabets.ukrainianCapital.IndexOf(from);

                int index = (current - _move) % Alphabets.ukrainianLen;

                if (index < 0)
                    index += Alphabets.ukrainianLen;

                return Alphabets.ukrainianCapital[index];
            }

            if (from > 64 && from < 91)
            {
                int current = from - 'A';

                int index = (current - _move) % Alphabets.englishLen;

                if (index < 0)
                    index += Alphabets.englishLen;

                return (char)(index + 'A');
            }

            if (from > 96 && from < 123)
            {
                int current = from - 'a';

                int index = (current - _move) % Alphabets.englishLen;

                if (index < 0)
                    index += Alphabets.englishLen;

                return (char)(index + 'a');
            }

            return from;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MoveBoxContent, out _move))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Provide a number as a key",
                    "Wrong input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
