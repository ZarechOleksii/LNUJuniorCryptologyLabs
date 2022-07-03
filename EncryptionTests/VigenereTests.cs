using Xunit;
using CryptologyLabs.Vigenere;

namespace EncryptionTests
{
    public class VigenereTests
    {
        [Theory]
        // A - does not affect characters
        [InlineData("AAA", "ABC", "ABC")]
        // With upper/lower cases
        [InlineData("BBB", "abc", "bcd")]
        [InlineData("BBB", "AbC", "BcD")]
        [InlineData("BBB", "ABC", "BCD")]
        // With unencryptable symbols
        [InlineData("BAB", "a,b", "b,c")]
        [InlineData("BAB", "a b", "b c")]
        // With ukrainian letters
        // A - does not affect characters
        [InlineData("ААА", "АБВ", "АБВ")]
        // With upper/lower cases
        [InlineData("БББ", "абв", "бвг")]
        [InlineData("БББ", "АбВ", "БвГ")]
        [InlineData("БББ", "АБВ", "БВГ")]
        // With unencryptable symbols
        [InlineData("БAБ", "а,б", "б,в")]
        [InlineData("БAБ", "а б", "б в")]
        // Combined languages
        [InlineData("BBBAБББ", "ABC АБВ", "BCD БВГ")]
        [InlineData("БDDБ", "БDdб", "ВGgв")]
        public void VigenereEncryptor_EncryptTest(string key, string source, string encrypted)
        {
            //arrange
            var encryptor = new VigenereEncryptor(key);

            //act
            var result = encryptor.Encrypt(source);

            //assert
            Assert.Equal(encrypted, result);
        }

        [Theory]
        // A - does not affect characters
        [InlineData("AAA", "ABC", "ABC")]
        // With upper/lower cases
        [InlineData("BBB", "abc", "bcd")]
        [InlineData("BBB", "AbC", "BcD")]
        [InlineData("BBB", "ABC", "BCD")]
        // With unencryptable symbols
        [InlineData("BAB", "a,b", "b,c")]
        [InlineData("BAB", "a b", "b c")]
        // With ukrainian letters
        // A - does not affect characters
        [InlineData("ААА", "АБВ", "АБВ")]
        // With upper/lower cases
        [InlineData("БББ", "абв", "бвг")]
        [InlineData("БББ", "АбВ", "БвГ")]
        [InlineData("БББ", "АБВ", "БВГ")]
        // With unencryptable symbols
        [InlineData("БAБ", "а,б", "б,в")]
        [InlineData("БAБ", "а б", "б в")]
        // Combined languages
        [InlineData("BBBAБББ", "ABC АБВ", "BCD БВГ")]
        [InlineData("БDDБ", "БDdб", "ВGgв")]
        public void VigenereEncryptor_DecryptTest(string key, string source, string encrypted)
        {
            //arrange
            var encryptor = new VigenereEncryptor(key);

            //act
            var result = encryptor.Decrypt(encrypted);

            //assert
            Assert.Equal(source, result);
        }

        [Theory]
        // A - does not affect characters
        [InlineData("AAA", "ABC", "ABC")]
        // With upper/lower cases
        [InlineData("BBB", "abc", "bcd")]
        [InlineData("BBB", "AbC", "BcD")]
        [InlineData("BBB", "ABC", "BCD")]
        // With unencryptable symbols
        [InlineData("BAB", "a,b", "b,c")]
        [InlineData("BAB", "a b", "b c")]
        // With ukrainian letters
        // A - does not affect characters
        [InlineData("ААА", "АБВ", "АБВ")]
        // With upper/lower cases
        [InlineData("БББ", "абв", "бвг")]
        [InlineData("БББ", "АбВ", "БвГ")]
        [InlineData("БББ", "АБВ", "БВГ")]
        // With unencryptable symbols
        [InlineData("БAБ", "а,б", "б,в")]
        [InlineData("БAБ", "а б", "б в")]
        // Combined languages
        [InlineData("BBBAБББ", "ABC АБВ", "BCD БВГ")]
        [InlineData("БDDБ", "БDdб", "ВGgв")]
        public void VigenereEncryptor_AttackTest(string key, string source, string encrypted)
        {
            //act
            var result = VigenereEncryptor.GetKey(source, encrypted);

            //assert
            Assert.Equal(key, result);
        }
    }
}
