using CryptologyLabs.Trithemius;
using Xunit;

namespace EncryptionTests
{
    public class TrithemiusTests
    {
        [Theory]
        //0 parameter do not change string
        [InlineData(0, 0, "abc", "abc")]
        //various parameters
        [InlineData(0, 1, "abc", "bcd")]
        [InlineData(0, -1, "abc", "zab")]
        [InlineData(1, 0, "abc", "ace")]
        [InlineData(-1, 0, "abc", "aaa")]
        [InlineData(1, 1, "abc", "bdf")]
        [InlineData(-1, -1, "abc", "zzz")]
        [InlineData(2, 0, "abc", "adg")]
        [InlineData(2, 1, "abc", "beh")]
        [InlineData(-2, 0, "abc", "azy")]
        [InlineData(-2, -1, "abc", "zyx")]
        //Symbols which are not English / Ukrainian letters do not change
        [InlineData(0, 1, "a2c", "b2d")]
        [InlineData(0, 1, "a c", "b d")]
        //Works with lower/upper case English/Ukrainian
        [InlineData(0, 1, "ABC", "BCD")]
        [InlineData(0, 1, "абв", "бвг")]
        [InlineData(0, 1, "АБВ", "БВГ")]
        public void LinearEncryptor_EncryptTest(int parA, int parB, string source, string encrypted)
        {
            //arrange
            var encryptor = new LinearEncryption(parA, parB);

            //act
            var result = encryptor.EncryptNoTable(source);

            //assert
            Assert.Equal(encrypted, result);
        }

        [Theory]
        //0 parameter do not change string
        [InlineData(0, 0, "abc", "abc")]
        //various parameters
        [InlineData(0, 1, "abc", "bcd")]
        [InlineData(0, -1, "abc", "zab")]
        [InlineData(1, 0, "abc", "ace")]
        [InlineData(-1, 0, "abc", "aaa")]
        [InlineData(1, 1, "abc", "bdf")]
        [InlineData(-1, -1, "abc", "zzz")]
        [InlineData(2, 0, "abc", "adg")]
        [InlineData(2, 1, "abc", "beh")]
        [InlineData(-2, 0, "abc", "azy")]
        [InlineData(-2, -1, "abc", "zyx")]
        //Symbols which are not English / Ukrainian letters do not change
        [InlineData(0, 1, "a2c", "b2d")]
        [InlineData(0, 1, "a c", "b d")]
        //Works with lower/upper case English/Ukrainian
        [InlineData(0, 1, "ABC", "BCD")]
        [InlineData(0, 1, "абв", "бвг")]
        [InlineData(0, 1, "АБВ", "БВГ")]
        public void LinearEncryptor_DencryptTest(int parA, int parB, string source, string encrypted)
        {
            //arrange
            var encryptor = new LinearEncryption(parA, parB);

            //act
            var result = encryptor.Decrypt(encrypted);

            //assert
            Assert.Equal(source, result);
        }

        [Theory]
        //0 parameter do not change string
        [InlineData(0, 0, 0, "abc", "abc")]
        //various parameters
        [InlineData(0, 0, 1, "abc", "bcd")]
        [InlineData(0, 0, -1, "abc", "zab")]
        [InlineData(0, 1, 0, "abc", "ace")]
        [InlineData(0, -1, 0, "abc", "aaa")]
        [InlineData(0, 1, 1, "abc", "bdf")]
        [InlineData(0, -1, -1, "abc", "zzz")]
        [InlineData(0, 2, 0, "abc", "adg")]
        [InlineData(0, 2, 1, "abc", "beh")]
        [InlineData(0, -2, 0, "abc", "azy")]
        [InlineData(0, -2, -1, "abc", "zyx")]
        [InlineData(1, 0, 0, "abc", "acg")]
        [InlineData(-1, 0, 0, "abc", "aay")]
        [InlineData(1, 0, 1, "abc", "bdh")]
        [InlineData(-1, 0, -1, "abc", "zzx")]
        [InlineData(1, 1, 1, "abc", "bej")]
        [InlineData(-1, -1, -1, "abc", "zyv")]
        [InlineData(2, 0, 0, "abc", "adk")]
        [InlineData(2, 0, 1, "abc", "bel")]
        [InlineData(2, 1, 1, "abc", "bfn")]
        [InlineData(2, 2, 1, "abc", "bgp")]
        [InlineData(-2, 0, 0, "abc", "azu")]
        [InlineData(4, 5, 2, "Is t", "Kd u")]
        //Symbols which are not English / Ukrainian letters do not change
        [InlineData(0, 0, 1, "a2c", "b2d")]
        [InlineData(0, 0, 1, "a c", "b d")]
        //Works with lower/upper case English/Ukrainian
        [InlineData(0, 0, 1, "ABC", "BCD")]
        [InlineData(0, 0, 1, "абв", "бвг")]
        [InlineData(0, 0, 1, "АБВ", "БВГ")]
        public void NonLinearEncryptor_EncryptTest(int parA, int parB, int parC, string source, string encrypted)
        {
            //arrange
            var encryptor = new NonLinearEncryption(parA, parB, parC);

            //act
            var result = encryptor.EncryptNoTable(source);

            //assert
            Assert.Equal(encrypted, result);
        }

        [Theory]
        //0 parameter do not change string
        [InlineData(0, 0, 0, "abc", "abc")]
        //various parameters
        [InlineData(0, 0, 1, "abc", "bcd")]
        [InlineData(0, 0, -1, "abc", "zab")]
        [InlineData(0, 1, 0, "abc", "ace")]
        [InlineData(0, -1, 0, "abc", "aaa")]
        [InlineData(0, 1, 1, "abc", "bdf")]
        [InlineData(0, -1, -1, "abc", "zzz")]
        [InlineData(0, 2, 0, "abc", "adg")]
        [InlineData(0, 2, 1, "abc", "beh")]
        [InlineData(0, -2, 0, "abc", "azy")]
        [InlineData(0, -2, -1, "abc", "zyx")]
        [InlineData(1, 0, 0, "abc", "acg")]
        [InlineData(-1, 0, 0, "abc", "aay")]
        [InlineData(1, 0, 1, "abc", "bdh")]
        [InlineData(-1, 0, -1, "abc", "zzx")]
        [InlineData(1, 1, 1, "abc", "bej")]
        [InlineData(-1, -1, -1, "abc", "zyv")]
        [InlineData(2, 0, 0, "abc", "adk")]
        [InlineData(2, 0, 1, "abc", "bel")]
        [InlineData(2, 1, 1, "abc", "bfn")]
        [InlineData(2, 2, 1, "abc", "bgp")]
        [InlineData(-2, 0, 0, "abc", "azu")]
        [InlineData(4, 5, 2, "Is t", "Kd u")]
        //Symbols which are not English / Ukrainian letters do not change
        [InlineData(0, 0, 1, "a2c", "b2d")]
        [InlineData(0, 0, 1, "a c", "b d")]
        //Works with lower/upper case English/Ukrainian
        [InlineData(0, 0, 1, "ABC", "BCD")]
        [InlineData(0, 0, 1, "абв", "бвг")]
        [InlineData(0, 0, 1, "АБВ", "БВГ")]
        public void NonLinearEncryptor_DecryptTest(int parA, int parB, int parC, string source, string encrypted)
        {
            //arrange
            var encryptor = new NonLinearEncryption(parA, parB, parC);

            //act
            var result = encryptor.Decrypt(encrypted);

            //assert
            Assert.Equal(source, result);
        }

        [Theory]
        //symbol "a" does not change string
        [InlineData("a", "abc", "abc")]
        //various parameters
        [InlineData("b", "abc", "bcd")]
        [InlineData("bc", "abc", "bdd")]
        [InlineData("bcd", "abc", "bdf")]
        [InlineData("aca", "abc", "adc")]
        //watchword longer than input
        [InlineData("bcdefg", "abc", "bdf")]
        //Symbols which are not English / Ukrainian letters do not change
        [InlineData("b", "a2c", "b2d")]
        [InlineData("b", "a c", "b d")]
        //Symbols which are not English / Ukrainian letters are skipped
        [InlineData("ab", "a aaa", "a bab")]
        [InlineData("ab", " a1aa1a", " a1ba1b")]
        //Works with lower/upper case English/Ukrainian as input
        [InlineData("b", "ABC", "BCD")]
        [InlineData("b", "абв", "бвг")]
        [InlineData("b", "АБВ", "БВГ")]
        //Works with lower/upper case English/Ukrainian as parameter
        [InlineData("B", "abc", "bcd")]
        [InlineData("б", "abc", "bcd")]
        [InlineData("Б", "abc", "bcd")]
        public void WatchwordEncryptor_EncryptTest(string watchword, string source, string encrypted)
        {
            //arrange
            var encryptor = new WatchwordEncryption(watchword);

            //act
            var result = encryptor.EncryptNoTable(source);

            //assert
            Assert.Equal(encrypted, result);
        }

        [Theory]
        //symbol "a" does not change string
        [InlineData("a", "abc", "abc")]
        //various parameters
        [InlineData("b", "abc", "bcd")]
        [InlineData("bc", "abc", "bdd")]
        [InlineData("bcd", "abc", "bdf")]
        [InlineData("aca", "abc", "adc")]
        //watchword longer than input
        [InlineData("bcdefg", "abc", "bdf")]
        //Symbols which are not English / Ukrainian letters do not change
        [InlineData("b", "a2c", "b2d")]
        [InlineData("b", "a c", "b d")]
        //Symbols which are not English / Ukrainian letters are skipped
        [InlineData("ab", "a aaa", "a bab")]
        [InlineData("ab", " a1aa1a", " a1ba1b")]
        //Works with lower/upper case English/Ukrainian as input
        [InlineData("b", "ABC", "BCD")]
        [InlineData("b", "абв", "бвг")]
        [InlineData("b", "АБВ", "БВГ")]
        //Works with lower/upper case English/Ukrainian as parameter
        [InlineData("B", "abc", "bcd")]
        [InlineData("б", "abc", "bcd")]
        [InlineData("Б", "abc", "bcd")]
        public void WatchwordEncryptor_DecryptTest(string watchword, string source, string encrypted)
        {
            //arrange
            var encryptor = new WatchwordEncryption(watchword);

            //act
            var result = encryptor.Decrypt(encrypted);

            //assert
            Assert.Equal(source, result);
        }
    }
}