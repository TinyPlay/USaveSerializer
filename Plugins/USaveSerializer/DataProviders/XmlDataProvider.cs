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
using System.Xml.Serialization;
using USaveLib.DataEncryption;
using UnityEngine;

namespace USaveLib.DataProviders
{
    /// <summary>
    /// XML Data provider for serialization / deserialization from file
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    internal class XmlDataProvider <TData> : IDataProvider<TData> where TData : new()
    {
        // Data Provider Params
        private readonly string _providerDataPath;
        private readonly Encoding _providerEncoding;
        private readonly IDataEncryptor _dataEncryptor;
        
        // Provider Data
        private TData _currentData;
        
        /// <summary>
        /// XML Data Provider
        /// </summary>
        /// <param name="dataPath"></param>
        /// <param name="dataEncoding"></param>
        /// <param name="dataEncryptor"></param>
        public XmlDataProvider(string dataPath, Encoding dataEncoding, IDataEncryptor dataEncryptor = null)
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
            TData readedModel = DeserializeObject<TData>(readedData);
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
            // Converted Xml
            string convertedData = SerializeObject(data);
            if(string.IsNullOrEmpty(convertedData))
                return;
            
            // Decrypt Data
            if (_dataEncryptor != null)
                convertedData = _dataEncryptor.EncodeString(convertedData);
            
            // Save Data
            File.WriteAllText(_providerDataPath, convertedData, _providerEncoding);
        }
        
        /// <summary>
        /// Serialize Object to String
        /// </summary>
        /// <param name="toSerialize"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        private string SerializeObject<TObject>(TObject toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            using StringWriter textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
        
        /// <summary>
        /// Deserialize Object from String
        /// </summary>
        /// <param name="deserializationString"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        private TObject DeserializeObject<TObject>(string deserializationString) where TObject : new()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TData));
            using StringReader textReader = new StringReader(deserializationString);
            var result = (TObject)xmlSerializer.Deserialize(textReader);
            return result;
        }
    }
}