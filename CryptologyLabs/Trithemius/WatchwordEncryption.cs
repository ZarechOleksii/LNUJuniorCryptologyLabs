using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CryptologyLabs.Trithemius
{
    public class WatchwordEncryption : BaseTrithemiusEncryption
    {
        private string? Watchword;

        public WatchwordEncryption() { }

        public WatchwordEncryption(string watchword)
        {
            Watchword = watchword;
        }

        public override string Encrypt(string sourceText)
        {
            int position = 0;

            List<char> chars = new();
            for (int i = 0; i < sourceText.Length; i++)
            {
                if (IsEncrypted(sourceText[i]))
                {
                    chars.Add(EncodeChar(sourceText[i], position));
                    position++;
                }
                else
                {
                    chars.Add(sourceText[i]);
                }
            }

            var toReturn = new string(chars.ToArray());

            encounters =
                toReturn.Where(v => IsEncrypted(v)).GroupBy(v => v).ToDictionary(x => x.Key, x => x.Count());
            var table = new LetterTable();
            table.PopulateData(encounters);
            table.Show();

            return toReturn;
        }

        public override string EncryptNoTable(string sourceText)
        {
            int position = 0;

            List<char> chars = new();
            for (int i = 0; i < sourceText.Length; i++)
            {
                if (IsEncrypted(sourceText[i]))
                {
                    chars.Add(EncodeChar(sourceText[i], position));
                    position++;
                }
                else
                {
                    chars.Add(sourceText[i]);
                }
            }

            var toReturn = new string(chars.ToArray());
            return toReturn;
        }

        public override string Decrypt(string encryptedText)
        {
            int position = 0;

            List<char> chars = new();
            for (int i = 0; i < encryptedText.Length; i++)
            {
                if (IsEncrypted(encryptedText[i]))
                {
                    chars.Add(DecodeChar(encryptedText[i], position));
                    position++;
                }
                else
                {
                    chars.Add(encryptedText[i]);
                }
            }

            var toReturn = new string(chars.ToArray());
            return toReturn;
        }

        protected override int ToMove(int position)
        {
            var symbol = Watchword[position % Watchword.Length];

            if (Alphabets.ukrainian.Contains(symbol))
                return Alphabets.ukrainian.IndexOf(symbol);

            if (Alphabets.ukrainianCapital.Contains(symbol))
                return Alphabets.ukrainianCapital.IndexOf(symbol);

            if (symbol > 64 && symbol < 91)
                return symbol - 'A';

            if (symbol > 96 && symbol < 123)
                return symbol - 'a';

            throw new Exception("How?");
        }

        public override string? Attack(string sourceText, string encryptedText)
        {
            string repeatingWatchwords = 
                new (
                    ClearText(sourceText)
                    .Zip(ClearText(encryptedText))
                    .AsParallel()
                    .AsOrdered()
                    .Select(v => Moved(v.First, v.Second))
                    .Select(v => Alphabets.ukrainianCapital[v])
                    .ToArray()
                    );

            int length = 1;
            var current = repeatingWatchwords[length..];
            var answer = false;

            while (length < repeatingWatchwords.Length)
            {
                current = repeatingWatchwords[length..];

                for(int i = 0; i < repeatingWatchwords.Length; i += length)
                {
                    if (i + length <= repeatingWatchwords.Length)
                    {
                        if (repeatingWatchwords.Substring(i, length) != current)
                        {
                            answer = false;
                            break;
                        }
                    }
                    answer = true;
                }
                if (answer)
                    break;
                else
                    length++;
            }

            Watchword = current;

            if (answer && encryptedText == EncryptNoTable(sourceText))
            {
                MessageBox.Show(
                    $"Watchword: {current}",
                    "Attack successful",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return current;
            }
            else
            {
                MessageBox.Show("Failed to find key",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
        }

        private static string ClearText(string givenText)
        {
            return new string(
                givenText
                .Replace("\n", "")
                .Replace("\r", "")
                .Where(x => IsEncrypted(x))
                .ToArray());
        }
    }
}
