using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CryptologyLabs.Trithemius
{
    public class LinearEncryption : BaseTrithemiusEncryption
    {
        public int CoefficientA;

        public int CoefficientB;

        public LinearEncryption() { }

        public LinearEncryption(int coefficientA, int coefficientB)
        {
            CoefficientA = coefficientA;
            CoefficientB = coefficientB;
        }

        protected override int ToMove(int position)
        {
            return CoefficientA * position + CoefficientB;
        }

        public override string? Attack(string sourceText, string encryptedText)
        {
            List<int> charIndexes = new();

            for (int i = 0; i < encryptedText.Length; i++)
            {
                if (IsEncrypted(encryptedText[i]))
                {
                    charIndexes.Add(i);
                }

                if (charIndexes.Count > 1)
                {
                    int p1 = charIndexes[0];
                    int p2 = charIndexes[1];

                    //get k for both encrypted chars
                    var moved1 = Moved(sourceText[p1], encryptedText[p1]);
                    var moved2 = Moved(sourceText[p2], encryptedText[p2]);

                    //calculate parA from equation system
                    int parA = (moved2 - moved1) / (p2 - p1);

                    //calculate parB from first equation
                    int parB = moved1 - (p1 * parA);

                    CoefficientA = parA;
                    CoefficientB = parB;

                    if (encryptedText == EncryptNoTable(sourceText))
                        MessageBox.Show($"Parameter A= {CoefficientA};\nParameter B = {CoefficientB}",
                            "Deciphered",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    else
                        MessageBox.Show("Failed to find key",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                    return $"{CoefficientA},{CoefficientB}";
                }
            }

            MessageBox.Show("Not enough encrypted characters to decrypt",
                "Indecipherable",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return null;
        }
    }
}
