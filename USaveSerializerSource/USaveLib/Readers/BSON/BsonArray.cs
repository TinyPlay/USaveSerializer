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
    /// BSON Array Class
    /// </summary>
    public class BsonArray : BsonValue, IEnumerable
    {
        // BSON Array List of Values
        private readonly List<BsonValue> _mList = new List<BsonValue> ();

        /// <summary>
        /// BSON Array Constructor
        /// </summary>
        public BsonArray () : base(BsonValue.ValueType.Array) { }

        #region Indexer
        public override BsonValue this [int index] => _mList [index];
        public int Count => _mList.Count;
        #endregion


        #region Methods
        public override void Add(BsonValue v) {
            _mList.Add (v);
        }

        public int IndexOf (BsonValue item) {
            return _mList.IndexOf (item);
        }
        public void Insert (int index, BsonValue item) {
            _mList.Insert (index, item);
        }
        public bool Remove(BsonValue v) {
            return _mList.Remove (v);
        }
        public void RemoveAt (int index) {
            _mList.RemoveAt (index);
        }
        public override void Clear() {
            _mList.Clear ();
        }
        public override bool Contains(BsonValue v) {
            return _mList.Contains (v);
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return _mList.GetEnumerator ();
        }
        #endregion
    }
}