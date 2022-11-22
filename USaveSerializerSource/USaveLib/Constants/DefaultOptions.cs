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
namespace USaveLib.Constants
{
    /// <summary>
    /// Default Options
    /// </summary>
    public static class DefaultOptions
    {
        public const string ApplicationSecret    =     "DefaultApplicationSecretKey";
        public const string PublicKey            =     "DefaultApplicationPublicKey";
        public const string PrivateKey           =     "DefaultApplicationPrivateKey";
    }
}