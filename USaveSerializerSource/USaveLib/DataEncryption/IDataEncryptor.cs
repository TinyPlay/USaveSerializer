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
namespace USaveLib.DataEncryption
{
    /// <summary>
    /// Data Encryptor Interface
    /// </summary>
    public interface IDataEncryptor
    {
        string DecodeString(string encodedString);
        byte[] DecodeBinary(byte[] encodedData);
        string EncodeString(string decodedString);
        byte[] EncodeBinary(byte[] decodedData);
    }
}