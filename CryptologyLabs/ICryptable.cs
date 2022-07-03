using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptologyLabs
{
    internal interface ICryptable
    {
        public bool? ShowDialog();

        public string Encrypt(string sourceText);

        public string Decrypt(string encryptedText);

        public string? Attack(string sourceText, string encryptedText);
    }
}
