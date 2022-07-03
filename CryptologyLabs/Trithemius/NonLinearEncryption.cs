using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CryptologyLabs.Trithemius
{
    public class NonLinearEncryption : BaseTrithemiusEncryption
    {
        public int CoefficientA;

        public int CoefficientB;

        public int CoefficientC;
        public NonLinearEncryption() { }

        public NonLinearEncryption(int coefficientA, int coefficientB, int coefficientC)
        {
            CoefficientA = coefficientA;
            CoefficientB = coefficientB;   
            CoefficientC = coefficientC;
        }

        protected override int ToMove(int position)
        {
            return (CoefficientA * position * position) + (CoefficientB * position) + CoefficientC;
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

                if (charIndexes.Count > 2)
                {
                    int p1 = charIndexes[0];
                    int p2 = charIndexes[1];
                    int p3 = charIndexes[2];

                    int iteration = 1;
                    int iter1 = 0;
                    int iter2 = 0;
                    int iter3 = 0;

                    do
                    {
                        //get k for both encrypted chars
                        var moved1 = Alphabets.IsUkrainian(sourceText[p1]) 
                            ? Moved(sourceText[p1], encryptedText[p1]) + (Alphabets.ukrainianLen * iter1)
                            : Moved(sourceText[p1], encryptedText[p1]) + (Alphabets.englishLen * iter1);
                        var moved2 = Alphabets.IsUkrainian(sourceText[p2])
                            ? Moved(sourceText[p2], encryptedText[p2]) + (Alphabets.ukrainianLen * iter2)
                            : Moved(sourceText[p2], encryptedText[p2]) + (Alphabets.englishLen * iter2);
                        var moved3 = Alphabets.IsUkrainian(sourceText[p3])
                            ? Moved(sourceText[p3], encryptedText[p3]) + (Alphabets.ukrainianLen * iter3)
                            : Moved(sourceText[p3], encryptedText[p3]) + (Alphabets.englishLen * iter3);

                        //result of eq1 - eq2
                        int diff1 = moved1 - moved2;
                        int numA1 = p1 * p1 - p2 * p2;
                        int numB1 = p1 - p2;

                        //result of eq2 - eq3
                        int diff2 = moved2 - moved3;
                        int numA2 = p2 * p2 - p3 * p3;
                        int numB2 = p2 - p3;

                        //calculate A from 2 previous equations
                        diff1 *= numB2;
                        numA1 *= numB2;
                        diff2 *= numB1;
                        numA2 *= numB1;
                        int parA = (diff1 - diff2) / (numA1 - numA2);

                        //substitute A in previous equation to get B
                        numB1 *= numB2;
                        int parB = (diff1 - (numA1 * parA)) / numB1;

                        //substitute A & B in starting equation to get C
                        int parC = moved1 - (p1 * p1 * parA) - (p1 * parB);

                        CoefficientA = parA;
                        CoefficientB = parB;
                        CoefficientC = parC;

                        Recalculate(ref iteration, ref iter1, ref iter2, ref iter3);
                    }
                    while (encryptedText != EncryptNoTable(sourceText));

                    if (encryptedText == EncryptNoTable(sourceText))
                        MessageBox.Show($"Parameter A= {CoefficientA};\nParameter B = {CoefficientB};\nParameter C = {CoefficientC}",
                            "Deciphered",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    else
                        MessageBox.Show("Failed to find key",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                    return $"{CoefficientA},{CoefficientB},{CoefficientC}";
                }
            }

            MessageBox.Show("Not enough encrypted characters to decrypt",
                "Indecipherable",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return null;
        }

        private static void Recalculate(ref int iteration, ref int iter1, ref int iter2, ref int iter3)
        {
            if (iter1 == iteration && iter2 == iteration && iter3 == iteration)
            {
                iteration++;
                iter3 = -iteration;
                iter2 = -iteration;
                iter1 = -iteration;
            }
            else
            {
                if (iter3 == iteration)
                {
                    if (iter2 == iteration)
                    {
                        iter1++;
                        iter2 = -iteration;
                        iter3 = -iteration;
                    }
                    else
                    {
                        iter2++;
                        iter3 = -iteration;
                    }
                }
                else
                {
                    iter3++;
                }
            }
        }
    }
}
