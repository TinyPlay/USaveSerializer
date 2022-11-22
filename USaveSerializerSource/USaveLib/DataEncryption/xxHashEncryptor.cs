﻿/*================================================================
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
    /// xxHash Data Encryptor Hash Class
    /// </summary>
    public class xxHashEncryptor : IDataEncryptor
    {
        public class EncodingOptions
        {
            public Encoding Encoding = Encoding.UTF8;
            public uint PRIME32_1 = 2654435761U;
            public uint PRIME32_2 = 2246822519U;
            public uint PRIME32_3 = 3266489917U;
            public uint PRIME32_4 = 668265263U;
            public uint PRIME32_5 = 374761393U;

            public int Length = 16;
            public uint Seed = 0;
        }
        private static EncodingOptions _encryptor = new EncodingOptions();
        
        /// <summary>
        /// xxHash Encryption Provider
        /// </summary>
        /// <param name="options"></param>
        public xxHashEncryptor(EncodingOptions options)
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
            return Convert.ToString(EncodeBinary(_encryptor.Encoding.GetBytes(plane)));
        }
        
        /// <summary>
        /// Encrypt Data Bytes
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public byte[] EncodeBinary(byte[] buf)
        {
            uint h32;
            int index = 0;

            if (_encryptor.Length >= 16)
            {
                int limit = _encryptor.Length - 16;
                uint v1 = _encryptor.Seed + _encryptor.PRIME32_1 + _encryptor.PRIME32_2;
                uint v2 = _encryptor.Seed + _encryptor.PRIME32_2;
                uint v3 = _encryptor.Seed;
                uint v4 = _encryptor.Seed - _encryptor.PRIME32_1;

                do
                {
                    uint read_value = (uint)(buf[index++] | buf[index++] << 8 | buf[index++] << 16 | buf[index++] << 24);
                    v1 += read_value * _encryptor.PRIME32_2;
                    v1 = (v1 << 13) | (v1 >> 19);
                    v1 *= _encryptor.PRIME32_1;

                    read_value = (uint)(buf[index++] | buf[index++] << 8 | buf[index++] << 16 | buf[index++] << 24);
                    v2 += read_value * _encryptor.PRIME32_2;
                    v2 = (v2 << 13) | (v2 >> 19);
                    v2 *= _encryptor.PRIME32_1;

                    read_value = (uint)(buf[index++] | buf[index++] << 8 | buf[index++] << 16 | buf[index++] << 24);
                    v3 += read_value * _encryptor.PRIME32_2;
                    v3 = (v3 << 13) | (v3 >> 19);
                    v3 *= _encryptor.PRIME32_1;

                    read_value = (uint)(buf[index++] | buf[index++] << 8 | buf[index++] << 16 | buf[index++] << 24);
                    v4 += read_value * _encryptor.PRIME32_2;
                    v4 = (v4 << 13) | (v4 >> 19);
                    v4 *= _encryptor.PRIME32_1;

                } while (index <= limit);

                h32 = ((v1 << 1) | (v1 >> 31)) + ((v2 << 7) | (v2 >> 25)) + ((v3 << 12) | (v3 >> 20)) + ((v4 << 18) | (v4 >> 14));
            }
            else
            {
                h32 = _encryptor.Seed + _encryptor.PRIME32_5;
            }

            h32 += (uint)_encryptor.Length;

            while (index <= _encryptor.Length - 4)
            {
                h32 += (uint)(buf[index++] | buf[index++] << 8 | buf[index++] << 16 | buf[index++] << 24) * _encryptor.PRIME32_3;
                h32 = ((h32 << 17) | (h32 >> 15)) * _encryptor.PRIME32_4;
            }

            while (index < _encryptor.Length)
            {
                h32 += buf[index] * _encryptor.PRIME32_5;
                h32 = ((h32 << 11) | (h32 >> 21)) * _encryptor.PRIME32_1;
                index++;
            }

            h32 ^= h32 >> 15;
            h32 *= _encryptor.PRIME32_2;
            h32 ^= h32 >> 13;
            h32 *= _encryptor.PRIME32_3;
            h32 ^= h32 >> 16;

            return BitConverter.GetBytes(h32);
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