using Xunit;
using CryptologyLabs.Gamma;
using System.Linq;
using System;

namespace EncryptionTests
{
    public class GammaTests
    {
        /*
            english
            binary A = 1000001
            binary B = 1000010
            binary C = 1000011

            binary a = 1100001
            binary b = 1100010
            binary c = 1100011

            ukrainian
            binary А = 10000010000
            binary Б = 10000010001
            binary В = 10000010010

            binary а = 10000110000
            binary б = 10000110001
            binary в = 10000110010
        */

        [Theory]
        [InlineData("0 0 0", "ABC", "ABC")]
        // changing last bit
        // 1000000 - @
        // 1000011 - C
        // 1000010 - B
        [InlineData("1 1 1", "ABC", "@CB")]
        // changing last bit
        // 1100000 - `
        // 1100011 - c
        // 1100010 - b
        [InlineData("1 1 1", "abc", "`cb")]
        // changing last bit
        // 10000010001 - Б
        // 10000010000 - А
        // 10000010011 - Г
        [InlineData("1 1 1", "АБВ", "БАГ")]
        // changing last bit
        // 10000110001 - а
        // 10000110010 - б
        // 10000110011 - в
        [InlineData("1 1 1", "абв", "баг")]
        // 1000001(A) XOR 0011101 = 1011100(\)
        [InlineData("0011101", "A", "\\")]
        // 1100001(a) XOR 0011101 = 1111100(|)
        [InlineData("0011101", "a", "|")]
        // 10000010000(A (ukr)) XOR 00000010011 = 10000000011(Ѓ)
        [InlineData("00000010011", "А", "Ѓ")]
        // 10000110000(а (ukr)) XOR 00000010011 = 10000100011(У)
        [InlineData("00000010011", "а", "У")]
        public void GammaEncryptor_EncryptTest(string gamma, string source, string encrypted)
        {
            var gammaString = new string(gamma
                .Split(' ')
                .Select(q => (char)Convert.ToInt32(q, 2))
                .ToArray());

            //arrange
            var encryptor = new GammaEncryptor(gammaString);

            //act
            var result = encryptor.Encrypt(source);

            //assert
            Assert.Equal(encrypted, result);
        }

        [Theory]
        [InlineData("0 0 0", "ABC", "ABC")]
        // changing last bit
        // 1000000 - @
        // 1000011 - C
        // 1000010 - B
        [InlineData("1 1 1", "ABC", "@CB")]
        // changing last bit
        // 1100000 - `
        // 1100011 - c
        // 1100010 - b
        [InlineData("1 1 1", "abc", "`cb")]
        // changing last bit
        // 10000010001 - Б
        // 10000010000 - А
        // 10000010011 - Г
        [InlineData("1 1 1", "АБВ", "БАГ")]
        // changing last bit
        // 10000110001 - а
        // 10000110010 - б
        // 10000110011 - в
        [InlineData("1 1 1", "абв", "баг")]
        // 1000001(A) XOR 0011101 = 1011100(\)
        [InlineData("0011101", "A", "\\")]
        // 1100001(a) XOR 0011101 = 1111100(|)
        [InlineData("0011101", "a", "|")]
        // 10000010000(A (ukr)) XOR 00000010011 = 10000000011(Ѓ)
        [InlineData("00000010011", "А", "Ѓ")]
        // 10000110000(а (ukr)) XOR 00000010011 = 10000100011(У)
        [InlineData("00000010011", "а", "У")]
        public void GammaEncryptor_DecryptTest(string gamma, string source, string encrypted)
        {
            var gammaString = new string(gamma
                .Split(' ')
                .Select(q => (char)Convert.ToInt32(q, 2))
                .ToArray());

            //arrange
            var encryptor = new GammaEncryptor(gammaString);

            //act
            var result = encryptor.Decrypt(encrypted);

            //assert
            Assert.Equal(source, result);
        }

        [Theory]
        [InlineData("0 0 0", "ABC", "ABC")]
        // changing last bit
        // 1000000 - @
        // 1000011 - C
        // 1000010 - B
        [InlineData("1 1 1", "ABC", "@CB")]
        // changing last bit
        // 1100000 - `
        // 1100011 - c
        // 1100010 - b
        [InlineData("1 1 1", "abc", "`cb")]
        // changing last bit
        // 10000010001 - Б
        // 10000010000 - А
        // 10000010011 - Г
        [InlineData("1 1 1", "АБВ", "БАГ")]
        // changing last bit
        // 10000110001 - а
        // 10000110010 - б
        // 10000110011 - в
        [InlineData("1 1 1", "абв", "баг")]
        // 1000001(A) XOR 0011101 = 1011100(\)
        [InlineData("0011101", "A", "\\")]
        // 1100001(a) XOR 0011101 = 1111100(|)
        [InlineData("0011101", "a", "|")]
        // 10000010000(A (ukr)) XOR 00000010011 = 10000000011(Ѓ)
        [InlineData("00000010011", "А", "Ѓ")]
        // 10000110000(а (ukr)) XOR 00000010011 = 10000100011(У)
        [InlineData("00000010011", "а", "У")]
        public void GammaEncryptor_AttackTest(string gamma, string source, string encrypted)
        {
            //arrange
            var gammaString = new string(gamma
                .Split(' ')
                .Select(q => (char)Convert.ToInt32(q, 2))
                .ToArray());

            //act
            var result = GammaEncryptor.GetGamma(source, encrypted);

            //assert
            Assert.Equal(gammaString, result);
        }
    }
}
