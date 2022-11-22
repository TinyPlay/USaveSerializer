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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using USaveLib.DataEncryption;

namespace USaveLib.DataProviders
{
    /// <summary>
    /// Binary Data Provider for Serialization / Deserialization from File
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    internal class BinaryDataProvider<TData> : IDataProvider<TData> where TData : new()
    {
        // Data Provider Params
        private readonly string _providerDataPath;
        private readonly IDataEncryptor _dataEncryptor;
        
        // Provider Data
        private TData _currentData;
        
        /// <summary>
        /// Binary Data Provider
        /// </summary>
        /// <param name="dataPath"></param>
        /// <param name="dataEncryptor"></param>
        public BinaryDataProvider(string dataPath, IDataEncryptor dataEncryptor = null)
        {
            _providerDataPath = Application.persistentDataPath + dataPath;
            _currentData = new TData();
            _dataEncryptor = dataEncryptor;
        }

        /// <summary>
        /// Load Game System Data
        /// </summary>
        public TData LoadData()
        {
            // Check File Exists
            if(!File.Exists(_providerDataPath))
                return _currentData;
            
            // Load Data
            byte[] readedData = File.ReadAllBytes(_providerDataPath);
            if(readedData.Length < 1)
                return _currentData;

            // Decrypt Data
            if (_dataEncryptor != null)
                readedData = _dataEncryptor.DecodeBinary(readedData);

            // Convert Loaded Model
            TData readedModel = Deserialize<TData>(readedData);
            if(readedModel == null)
                return _currentData;
            
            // Set New Model
            _currentData = readedModel;
            return _currentData;
        }

        /// <summary>
        /// Get Current Deserialized Data
        /// </summary>
        /// <returns></returns>
        public TData GetData()
        {
            return _currentData;
        }

        /// <summary>
        /// Save Game System Data
        /// </summary>
        public void SaveData(TData data)
        {
            // Converted Binary
            byte[] convertedData = Serialize(data);
            if(convertedData.Length < 1)
                return;
            
            // Decrypt Data
            if (_dataEncryptor != null)
                convertedData = _dataEncryptor.EncodeBinary(convertedData);
            
            // Save Data
            File.WriteAllBytes(_providerDataPath, convertedData);
        }
        
        /// <summary>
        /// Deserialize Binary Data
        /// </summary>
        /// <param name="param"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T Deserialize<T>(byte[] param)
        {
            using (MemoryStream ms = new MemoryStream(param))
            {
                IFormatter br = new BinaryFormatter();
                return (T)br.Deserialize(ms);
            }
        }

        /// <summary>
        /// Serialize to Binary Array
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private byte[] Serialize<T>(T objectToSerialize)
        {
            if(objectToSerialize == null)
                return null;
            
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, objectToSerialize);
                return ms.ToArray();
            }
        }
    }
}