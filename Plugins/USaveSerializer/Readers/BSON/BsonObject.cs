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
using System.Collections;
using System.Collections.Generic;

namespace USaveLib.Readers.BSON
{
    /// <summary>
    /// BSON Object Class
    /// </summary>
    public class BsonObject : BsonValue, IEnumerable
    {
        // BSON Object Mapping
        private readonly Dictionary<string, BsonValue> _mMap = new Dictionary<string, BsonValue>();
        
        /// <summary>
        /// Object Constructor
        /// </summary>
        public BsonObject () : base(BsonValue.ValueType.Object) { }

        #region BSON Object Properties
        public ICollection<string> Keys
        {
            get { return _mMap.Keys; }
        }
        public ICollection<BsonValue> Values
        {
            get { return _mMap.Values; }
        }
        public int Count => _mMap.Count;
        #endregion


        #region BSON Object Indexer
        public override BsonValue this [string key]
        {
            get => _mMap [key];
        }
        #endregion

        #region BSON Object Methods
        public override void Clear() {
            _mMap.Clear();
        }
        public override void Add (string key, BsonValue value) {
            _mMap.Add (key, value);
        }
		

        public override bool Contains(BsonValue v) {
            return _mMap.ContainsValue (v);
        }
        public override bool ContainsKey (string key) {
            return _mMap.ContainsKey(key);
        }

        public bool Remove (string key) {
            return _mMap.Remove (key);
        }

        public bool TryGetValue (string key, out BsonValue value) {
            return _mMap.TryGetValue (key, out value);
        }


        IEnumerator IEnumerable.GetEnumerator() {
            return _mMap.GetEnumerator ();
        }
        #endregion
    }
}