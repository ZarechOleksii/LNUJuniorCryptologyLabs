using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptologyLabs.Trithemius
{
    public abstract class BaseTrithemiusEncryption
    {
        protected abstract int ToMove(int position);

        protected Dictionary<char, int> encounters = new();

        public abstract string? Attack(string sourceText, string encryptedText);

        public virtual string Decrypt(string encryptedText)
        {
            return new string(encryptedText.Select((v, i) => DecodeChar(v, i)).ToArray());
        }

        public virtual string Encrypt(string sourceText)
        {
            var toReturn = new string(sourceText.Select((v, i) => EncodeChar(v, i)).ToArray());

            encounters = 
                toReturn.Where(v => IsEncrypted(v)).GroupBy(v => v).ToDictionary(x => x.Key, x => x.Count());
            var table = new LetterTable();
            table.PopulateData(encounters);
            table.Show();

            return toReturn;
        }

        public virtual string EncryptNoTable(string sourceText)
        {
            var toReturn = new string(sourceText.Select((v, i) => EncodeChar(v, i)).ToArray());
            return toReturn;
        }

        protected virtual char EncodeChar(char from, int position)
        {
            if (Alphabets.ukrainian.Contains(from))
            {
                var current = Alphabets.ukrainian.IndexOf(from);

                int index = (current + ToMove(position)) % Alphabets.ukrainianLen;

                if (index < 0)
                    index += Alphabets.ukrainianLen;

                return Alphabets.ukrainian[index];
            }

            if (Alphabets.ukrainianCapital.Contains(from))
            {
                var current = Alphabets.ukrainianCapital.IndexOf(from);


                int index = (current + ToMove(position)) % Alphabets.ukrainianLen;

                if (index < 0)
                    index += Alphabets.ukrainianLen;

                return Alphabets.ukrainianCapital[index];
            }

            if (from > 64 && from < 91)
            {
                int current = from - 'A';


                int index = (current + ToMove(position)) % Alphabets.englishLen;

                if (index < 0)
                    index += Alphabets.englishLen;

                return (char)(index + 'A');
            }

            if (from > 96 && from < 123)
            {
                int current = from - 'a';


                int index = (current + ToMove(position)) % Alphabets.englishLen;

                if (index < 0)
                    index += Alphabets.englishLen;

                return (char)(index + 'a');
            }

            return from;
        }

        protected char DecodeChar(char from, int position)
        {
            if (Alphabets.ukrainian.Contains(from))
            {
                var current = Alphabets.ukrainian.IndexOf(from);

                int index = (current - ToMove(position)) % Alphabets.ukrainianLen;

                if (index < 0)
                    index += Alphabets.ukrainianLen;

                return Alphabets.ukrainian[index];
            }

            if (Alphabets.ukrainianCapital.Contains(from))
            {
                var current = Alphabets.ukrainianCapital.IndexOf(from);

                int index = (current - ToMove(position)) % Alphabets.ukrainianLen;

                if (index < 0)
                    index += Alphabets.ukrainianLen;

                return Alphabets.ukrainianCapital[index];
            }

            if (from > 64 && from < 91)
            {
                int current = from - 'A';

                int index = (current - ToMove(position)) % Alphabets.englishLen;

                if (index < 0)
                    index += Alphabets.englishLen;

                return (char)(index + 'A');
            }

            if (from > 96 && from < 123)
            {
                int current = from - 'a';

                int index = (current - ToMove(position)) % Alphabets.englishLen;

                if (index < 0)
                    index += Alphabets.englishLen;

                return (char)(index + 'a');
            }

            return from;
        }

        protected static bool IsEncrypted(char letter)
        {
            if (Alphabets.ukrainian.Contains(letter)
                    || Alphabets.ukrainianCapital.Contains(letter)
                    || (letter > 64 && letter < 91)
                    || (letter > 96 && letter < 123))
                return true;
            return false;
        }

        protected virtual int Moved(char before, char after)
        {
            if ((after > 64 && after < 91) || (after > 96 && after < 123))
            {
                return (after - before < 0) ? (after - before + Alphabets.englishLen) : (after - before);
            }
            if (Alphabets.ukrainian.Contains(after))
            {
                var tempMoved = Alphabets.ukrainian.IndexOf(after) - Alphabets.ukrainian.IndexOf(before);

                return (tempMoved < 0) ? (tempMoved + Alphabets.ukrainianLen) : tempMoved;
            }
            if (Alphabets.ukrainianCapital.Contains(after))
            {
                var tempMoved = Alphabets.ukrainianCapital.IndexOf(after) - Alphabets.ukrainianCapital.IndexOf(before);

                return (tempMoved < 0) ? (tempMoved + Alphabets.ukrainianLen) : tempMoved;
            }
            return 0;
        }
    }
}
