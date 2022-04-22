// http://andang72.blogspot.com/2016/12/blog-post.html

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Tools
{
    /// <summary>
    /// AES 암호화 툴.
    /// </summary>
    public sealed class AesCiperTool
    {
        #region Fields

        private readonly static string HEAD = "Salted__";
        private readonly static string SALT = "PLUGININ";

        #endregion

        #region Public methods

        /// <summary>
        /// AES/CBC/PKCS5Padding 암호화 변환.
        /// 참고 사이트의 변환은 자바용 코드입니다. 자바로 제작 된 서버의 복호화 방식에 맞게 암호화 코드를 작성합니다.
        /// </summary>
        /// <param name="ciphertext">타겟 문자열.</param>
        /// <param name="passphrase">암호화 키.</param>
        /// <returns>변환 된 문자열.</returns>
        public static string Encrypt(string ciphertext, string passphrase)
        {
            string headerString = string.Format("{0}{1}", HEAD, SALT);
            int keySize = 256;
            int ivSize = 128;

            // 타겟 텍스트를 BASE64 형식으로 디코드 한다.
            _ = Encoding.UTF8.GetBytes(ciphertext);


            // 솔트를 구한다. (생략된 8비트는 Salted__ 시작되는 문자열이다.)
            byte[] saltBytes = Encoding.UTF8.GetBytes(SALT);

            // 전달 된 시큐어키값과 솔트값으로 키와 IV값을 가져온다.
            byte[] key = new byte[keySize / 8];
            byte[] iv = new byte[ivSize / 8];
            EvpKDF(Encoding.UTF8.GetBytes(passphrase), keySize, ivSize, saltBytes, key, iv);

            byte[] recoveredPlaintextBytes = null;
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.Key = key;
                myRijndael.IV = iv;
                myRijndael.Mode = CipherMode.CBC;
                myRijndael.Padding = PaddingMode.PKCS7;

                recoveredPlaintextBytes = EncryptStringToBytes(ciphertext, myRijndael.Key, myRijndael.IV);
            }

            byte[] headerByteArray = Encoding.UTF8.GetBytes(headerString);
            byte[] result = new byte[headerByteArray.Length + recoveredPlaintextBytes.Length];

            Array.Copy(headerByteArray, 0, result, 0, headerByteArray.Length);
            Array.Copy(recoveredPlaintextBytes, 0, result, headerByteArray.Length, recoveredPlaintextBytes.Length);

            return Convert.ToBase64String(result);
        }

        #endregion

        #region Private methods

        private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        private static byte[] EvpKDF(byte[] password, int keySize, int ivSize, byte[] salt, byte[] resultKey, byte[] resultIv)
        {
            keySize = keySize / 32;
            ivSize = ivSize / 32;

            int targetKeySize = keySize + ivSize;
            byte[] derivedBytes = new byte[targetKeySize * 4];
            int numberOfDerivedWords = 0;
            byte[] block = null;
            List<byte> hashList = new List<byte>();

            while (numberOfDerivedWords<targetKeySize)
            {
                if (block != null) {
                    hashList.AddRange(block);
                }

                hashList.AddRange(password);
                hashList.AddRange(salt);

                block = MD5.Create().ComputeHash(hashList.ToArray());
                hashList.Clear();

                Array.Copy(block, 0, derivedBytes, numberOfDerivedWords* 4, Math.Min(block.Length, (targetKeySize - numberOfDerivedWords) * 4));
                numberOfDerivedWords += block.Length / 4;
            }

            Array.Copy(derivedBytes, 0, resultKey, 0, keySize* 4);
            Array.Copy(derivedBytes, keySize* 4, resultIv, 0, ivSize* 4);

            return derivedBytes; // key + iv
        }

        #endregion
    }
}
