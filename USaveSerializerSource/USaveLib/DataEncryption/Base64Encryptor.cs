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
    /// Base64 Encryptor Class
    /// </summary>
    public class Base64Encryptor : IDataEncryptor
    {
        public class EncodingOptions
        {
            public Encoding Encoding = Encoding.UTF8;
        }
        private static EncodingOptions _encryptor = new EncodingOptions();

        /// <summary>
        /// Base64 Constructor
        /// </summary>
        public Base64Encryptor(EncodingOptions options)
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
            byte[] bytesToEncode = _encryptor.Encoding.GetBytes (plane);
            string encodedText = Convert.ToBase64String (bytesToEncode);
            return encodedText;
        }
        
        /// <summary>
        /// Encrypt Data Bytes
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public byte[] EncodeBinary(byte[] src)
        {
            string encodedText = Convert.ToBase64String(src);
            byte[] encodedBytes = _encryptor.Encoding.GetBytes (encodedText);
            return encodedBytes;
        }
        
        /// <summary>
        /// Decrypt String
        /// </summary>
        /// <param name="encrtpted"></param>
        /// <returns></returns>
        public string DecodeString(string encrtpted)
        {
            byte[] decodedBytes = Convert.FromBase64String (encrtpted);
            string decodedText = _encryptor.Encoding.GetString (decodedBytes);
            return decodedText;
        }
        
        /// <summary>
        /// Decrypt Binary Data
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public byte[] DecodeBinary(byte[] src)
        {
            string encoded = _encryptor.Encoding.GetString(src);
            byte[] decodedBytes = Convert.FromBase64String(encoded);
            return decodedBytes;
        }
    }
}