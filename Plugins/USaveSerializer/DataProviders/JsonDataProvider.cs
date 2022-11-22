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
using System.Text;
using UnityEngine;
using USaveLib.DataEncryption;

namespace USaveLib.DataProviders
{
    /// <summary>
    /// JSON Data Provider
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    internal class JsonDataProvider<TData> : IDataProvider<TData> where TData : new()
    {
        // Data Provider Params
        private readonly string _providerDataPath;
        private readonly Encoding _providerEncoding;
        private readonly IDataEncryptor _dataEncryptor;
        
        // Provider Data
        private TData _currentData;

        /// <summary>
        /// JSON Data Provider
        /// </summary>
        /// <param name="dataPath"></param>
        /// <param name="dataEncoding"></param>
        /// <param name="dataEncryptor"></param>
        public JsonDataProvider(string dataPath, Encoding dataEncoding, IDataEncryptor dataEncryptor = null)
        {
            _providerDataPath = Application.persistentDataPath + dataPath;
            _providerEncoding = dataEncoding;
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
            string readedData = File.ReadAllText(_providerDataPath, _providerEncoding);
            if(string.IsNullOrEmpty(readedData))
                return _currentData;

            // Decrypt Data
            if (_dataEncryptor != null)
                readedData = _dataEncryptor.DecodeString(readedData);

            // Convert Loaded Model
            TData readedModel = JsonUtility.FromJson<TData>(readedData);
            if(readedModel == null)
                return _currentData;
            
            // Set New Model
            _currentData = readedModel;
            return _currentData;
        }

        /// <summary>
        /// Save Game System Data
        /// </summary>
        public void SaveData(TData data)
        {
            // Converted JSON
            string convertedData = JsonUtility.ToJson((TData)data);
            if(string.IsNullOrEmpty(convertedData))
                return;
            
            // Decrypt Data
            if (_dataEncryptor != null)
                convertedData = _dataEncryptor.EncodeString(convertedData);
            
            // Save Data
            File.WriteAllText(_providerDataPath, convertedData, _providerEncoding);
        }
        
        /// <summary>
        /// Get Current Deserialized Data
        /// </summary>
        /// <returns></returns>
        public TData GetData()
        {
            return _currentData;
        }
    }
}