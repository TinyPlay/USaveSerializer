/*================================================================
/*  Unity Save Serializer Library
/*  This is a simple solution for data encryption / decryption and
 *  serialization / deserialization for your Unity games.
 *
 *  Save your serialized classes via binary, json or xml and
 *  encrypt via AES/RSA etc.
 *
 *  @developer      https://github.com/TinyPlay
 *  @version        1.0
 *  @build          100
 *  @url            https://tinyplay.games/
/*================================================================*/

using System;
using System.Text;

namespace USaveLib.DataEncryption
{
    /// <summary>
    /// MD5 Hash Encryptor Class
    /// </summary>
    public class Md5Encryptor : IDataEncryptor
    {
        public class EncodingOptions
        {
            public Encoding Encoding = Encoding.UTF8;
        }
        private static EncodingOptions _encryptor = new EncodingOptions();

        /// <summary>
        /// MD5 Hash Encryptor Constructor
        /// </summary>
        public Md5Encryptor(EncodingOptions options)
        {
            _encryptor = options;
        }
        
        /// <summary>
        /// Encode String Data
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public string EncodeString(string plane)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = _encryptor.Encoding.GetBytes(plane);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                
                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        
        /// <summary>
        /// Encrypt Data Bytes
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public byte[] EncodeBinary(byte[] src)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(src);
                return hashBytes;
            }
        }
        
        /// <summary>
        /// Decrypt String
        /// </summary>
        /// <param name="encrtpted"></param>
        /// <returns></returns>
        public string DecodeString(string encrtpted)
        {
            throw new Exception("You can't decode MD5 hash. Please, replace encoding provider.");
        }
        
        /// <summary>
        /// Decrypt Binary Data
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public byte[] DecodeBinary(byte[] src)
        {
            throw new Exception("You can't decode MD5 hash. Please, replace encoding provider.");
        }
    }
}