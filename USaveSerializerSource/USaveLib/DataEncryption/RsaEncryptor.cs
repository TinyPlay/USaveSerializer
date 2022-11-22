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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using USaveLib.Constants;

namespace USaveLib.DataEncryption
{
    /// <summary>
    /// RSA Encryptor Class
    /// </summary>
    public class RsaEncryptor
    {
        public class EncodingOptions
        {
            public string PublicKey = DefaultOptions.PublicKey;
            public string PrivateKey = DefaultOptions.PrivateKey;
            public Encoding Encoding = Encoding.UTF8;
        }
        private static EncodingOptions _encryptor = new EncodingOptions();
        
        /// <summary>
        /// RSA Encryption Provider
        /// </summary>
        /// <param name="options"></param>
        public RsaEncryptor(EncodingOptions options)
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
            byte[] encrypted = EncodeBinary(_encryptor.Encoding.GetBytes(plane));
            return Convert.ToBase64String(encrypted);
        }
        
        /// <summary>
        /// Encrypt Data Bytes
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public byte[] EncodeBinary(byte[] src)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(_encryptor.PublicKey);
                byte[] encrypted = rsa.Encrypt(src, false);
                return encrypted;
            }
        }
        
        /// <summary>
        /// Decrypt String
        /// </summary>
        /// <param name="encrtpted"></param>
        /// <returns></returns>
        public string DecodeString(string encrtpted)
        {
            byte[] decripted = DecodeBinary(Convert.FromBase64String(encrtpted));
            return _encryptor.Encoding.GetString(decripted);
        }
        
        /// <summary>
        /// Decrypt Binary Data
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public byte[] DecodeBinary(byte[] src)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(_encryptor.PrivateKey);
                byte[] decrypted = rsa.Decrypt(src, false);
                return decrypted;
            }
        }
        
        /// <summary>
        /// Generate Key Pair
        /// </summary>
        /// <param name="keySize"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string> GenrateKeyPair(int keySize){
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);
            return new KeyValuePair<string, string>(publicKey, privateKey);
        }
    }
}