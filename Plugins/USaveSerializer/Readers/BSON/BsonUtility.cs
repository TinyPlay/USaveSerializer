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
using System.IO;
using System.Text;

namespace USaveLib.Readers.BSON
{
    /// <summary>
    /// BSON Utility General Class
    /// </summary>
    public class BsonUtility
    {
	    // Stream Utilities
	    private readonly BinaryReader _mBinaryReader;
        private BinaryWriter _mBinaryWriter;
        
        /// <summary>
        /// Load BSON Object from Bytes Buffer
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static BsonObject FromBytes(byte[] buf) {
	        BsonUtility bson = new BsonUtility (buf);
	        return bson.DecodeDocument ();
		}

        /// <summary>
        /// Load BSON Object from string
        /// </summary>
        /// <param name="bsonDocument"></param>
        /// <returns></returns>
        public static BsonObject FromString(string bsonDocument)
        {
	        byte[] bsonBytes = Encoding.UTF8.GetBytes(bsonDocument);
	        return FromBytes(bsonBytes);
        }
        
        /// <summary>
        /// Load BSON Object of Type from Bytes
        /// </summary>
        /// <param name="buf"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static TObject FromBytes<TObject>(byte[] buf) where TObject : class
        {
	        BsonObject loadedObject = FromBytes(buf);
	        return loadedObject as TObject;
        }
        
        /// <summary>
        /// Load BSON Object of Type from String
        /// </summary>
        /// <param name="bsonDocument"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static TObject FromString<TObject>(string bsonDocument) where TObject : class
        {
	        BsonObject loadedObject = FromString(bsonDocument);
	        return loadedObject as TObject;
        }

        /// <summary>
        /// Serialize BSON Object from Bytes
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public static byte[] SerializeToBytes(BsonObject obj) {
			
			BsonUtility bson = new BsonUtility ();
			MemoryStream ms = new MemoryStream ();

			bson.EncodeDocument (ms, obj);

			byte[] buf = new byte[ms.Position];
			ms.Seek (0, SeekOrigin.Begin);
			var read = ms.Read (buf, 0, buf.Length);
			return buf;
		}

        /// <summary>
        /// Serialize BSON Object to String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(BsonObject obj)
        {
	        byte[] bsonBytes = SerializeToBytes(obj);
	        return Convert.ToString(bsonBytes);
        }

        /// <summary>
        /// Serialize to Bytes of generic type
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static byte[] SerializeToBytes<TObject>(TObject obj) where TObject : class
        {
	        BsonObject bsonConverted = new BsonObject();
	        bsonConverted = obj as BsonObject;
	        return SerializeToBytes(bsonConverted);
        }
        
        /// <summary>
        /// Serialize to String of generic type
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static string SerializeToString<TObject>(TObject obj) where TObject : class
        {
	        BsonObject bsonConverted = new BsonObject();
	        bsonConverted = obj as BsonObject;
	        return SerializeToString(bsonConverted);
        }

        /// <summary>
        /// BSON Utility Constructor
        /// </summary>
        /// <param name="buf"></param>
		private BsonUtility(byte[] buf = null)
		{
			MemoryStream mMemoryStream;
			if (buf != null) {
				mMemoryStream = new MemoryStream (buf);
				_mBinaryReader = new BinaryReader (mMemoryStream);
			} else {
				mMemoryStream = new MemoryStream ();
				_mBinaryWriter = new BinaryWriter (mMemoryStream);
			}
		}

        /// <summary>
        /// Decode BSON Element
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
		private BsonValue DecodeElement(out string name) {
			byte elementType = _mBinaryReader.ReadByte ();

			if(elementType == 0x01) { // Double
				name = DecodeCString ();
				return new BsonValue (_mBinaryReader.ReadDouble());

			} else if(elementType == 0x02) { // String
				name = DecodeCString ();
				return new BsonValue (DecodeString());

			} else if(elementType == 0x03) { // Document
				name = DecodeCString ();
				return DecodeDocument ();

			} else if(elementType == 0x04) { // Array
				name = DecodeCString ();
				return DecodeArray ();

			} else if(elementType == 0x05) { // Binary
				name = DecodeCString ();
				int length = _mBinaryReader.ReadInt32 ();
				byte binaryType = _mBinaryReader.ReadByte ();

				return new BsonValue(_mBinaryReader.ReadBytes (length));

			} else if(elementType == 0x08) { // Boolean
				name = DecodeCString ();
				return new BsonValue (_mBinaryReader.ReadBoolean());

			} else if(elementType == 0x09) { // DateTime
				name = DecodeCString ();
				Int64 time = _mBinaryReader.ReadInt64 ();
				return new BsonValue (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + new TimeSpan(time*10000));
			} else if(elementType == 0x0A) { // None
				name = DecodeCString ();
				return new BsonValue ();
			} else if(elementType == 0x10) { // Int32
				name = DecodeCString ();
				return new BsonValue (_mBinaryReader.ReadInt32());
			} else if(elementType == 0x12) { // Int64
				name = DecodeCString ();
				return new BsonValue (_mBinaryReader.ReadInt64());
			}

			
			throw new Exception($"Don't know elementType={elementType}");
		}

        /// <summary>
        /// Decode BSON Document
        /// </summary>
        /// <returns></returns>
		private BsonObject DecodeDocument() {
			int length = _mBinaryReader.ReadInt32 ()-4;

			BsonObject obj = new BsonObject ();

			int i = (int)_mBinaryReader.BaseStream.Position;
			while(_mBinaryReader.BaseStream.Position < i+length - 1) {
				string name;
				BsonValue value = DecodeElement (out name);
				obj.Add (name, value);

			}

			_mBinaryReader.ReadByte (); // zero
			return obj;
		}

        /// <summary>
        /// Decode BSON Array
        /// </summary>
        /// <returns></returns>
		private BsonArray DecodeArray() {
			BsonObject obj = DecodeDocument ();

			int i = 0;
			BsonArray array = new BsonArray ();
			while(obj.ContainsKey(Convert.ToString(i))) {
				array.Add (obj [Convert.ToString(i)]);

				i += 1;
			}

			return array;
		}

        /// <summary>
        /// Decode BSON String
        /// </summary>
        /// <returns></returns>
		private string DecodeString() {
			int length = _mBinaryReader.ReadInt32 ();
			byte []buf = _mBinaryReader.ReadBytes (length);
			
			return Encoding.UTF8.GetString (buf);
		}

        /// <summary>
        /// Decode BSON CString
        /// </summary>
        /// <returns></returns>
		private string DecodeCString() {

			var ms = new MemoryStream ();
			while (true) {
				byte buf = (byte)_mBinaryReader.ReadByte ();
				if (buf == 0)
					break;
				ms.WriteByte (buf);
			}

			return Encoding.UTF8.GetString (ms.GetBuffer (), 0, (int)ms.Position);
		}
        
        /// <summary>
        /// Encode BSON Element
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="name"></param>
        /// <param name="v"></param>
		private void EncodeElement(MemoryStream ms, string name, BsonValue v) {
			switch (v.valueType) {
			case BsonValue.ValueType.Double:
				ms.WriteByte (0x01);
				EncodeCString (ms, name);
				EncodeDouble (ms, v.doubleValue);
				return;
				case BsonValue.ValueType.String:
				ms.WriteByte (0x02);
				EncodeCString (ms, name);
				EncodeString (ms, v.stringValue);
				return;
				case BsonValue.ValueType.Object:
				ms.WriteByte (0x03);
				EncodeCString (ms, name);
				EncodeDocument(ms, v as BsonObject);
				return;
				case BsonValue.ValueType.Array:
				ms.WriteByte (0x04);
				EncodeCString (ms, name);
				EncodeArray (ms, v as BsonArray);
				return;
				case BsonValue.ValueType.Binary:
				ms.WriteByte (0x05);
				EncodeCString (ms, name);
				EncodeBinary (ms, v.binaryValue);
				return;
				case BsonValue.ValueType.Boolean:
					ms.WriteByte (0x08);
					EncodeCString (ms, name);
					EncodeBool (ms, v.boolValue);
				return;
				case BsonValue.ValueType.UtcDateTime:
				ms.WriteByte (0x09);
				EncodeCString (ms, name);
				EncodeUtcDateTime (ms, v.dateTimeValue);
				return;
				case BsonValue.ValueType.None:
				ms.WriteByte (0x0A);
				EncodeCString (ms, name);
				return;
				case BsonValue.ValueType.Int32:
				ms.WriteByte (0x10);
				EncodeCString (ms, name);
				EncodeInt32 (ms, v.int32Value);
				return;
				case BsonValue.ValueType.Int64:
				ms.WriteByte (0x12);
				EncodeCString (ms, name);
				EncodeInt64 (ms, v.int64Value);
				return;
			};
		}

        /// <summary>
        /// Encode BSON Document
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="obj"></param>
		private void EncodeDocument(MemoryStream ms, BsonObject obj) {

			MemoryStream dms = new MemoryStream ();
			foreach(string str in obj.Keys) {
				EncodeElement(dms, str, obj[str]);
			}

			BinaryWriter bw = new BinaryWriter (ms);
			bw.Write ((Int32)(dms.Position+4+1));
			bw.Write (dms.GetBuffer (), 0, (int)dms.Position);
			bw.Write ((byte)0);
		}

        /// <summary>
        /// Encode BSON Array
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="lst"></param>
		private void EncodeArray(MemoryStream ms, BsonArray lst) {

			var obj = new BsonObject ();
			for(int i = 0;i < lst.Count;++i) {
				obj.Add(Convert.ToString(i), lst[i]);
			}

			EncodeDocument (ms, obj);
		}

        /// <summary>
        /// Encode BSON Binary
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="buf"></param>
		private void EncodeBinary(MemoryStream ms, byte []buf) {
			byte []aBuf = BitConverter.GetBytes (buf.Length);
			ms.Write (aBuf, 0, aBuf.Length);
			ms.WriteByte (0);
			ms.Write (buf, 0, buf.Length);
		}

        /// <summary>
        /// Encode BSON CString
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="v"></param>
		private void EncodeCString(MemoryStream ms, string v) {
			byte []buf = new UTF8Encoding ().GetBytes (v);
			ms.Write (buf, 0, buf.Length);
			ms.WriteByte (0);
		}

        /// <summary>
        /// Encode BSON String
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="v"></param>
		private void EncodeString(MemoryStream ms, string v) {
			byte []strBuf = new UTF8Encoding ().GetBytes (v);
			byte[] buf = BitConverter.GetBytes (strBuf.Length+1);

			ms.Write (buf, 0, buf.Length);
			ms.Write (strBuf, 0, strBuf.Length);
			ms.WriteByte (0);
		}

        /// <summary>
        /// Encode BSON Double
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="v"></param>
		private void EncodeDouble(MemoryStream ms, double v) {
			byte []buf = BitConverter.GetBytes (v);
			ms.Write (buf, 0, buf.Length);
		}

        /// <summary>
        /// Encode BSON Bool
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="v"></param>
		private void EncodeBool(MemoryStream ms, bool v) {
			byte []buf = BitConverter.GetBytes (v);
			ms.Write (buf, 0, buf.Length);
		}
		
        /// <summary>
        /// Encode BSON Int32
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="v"></param>
		private void EncodeInt32(MemoryStream ms, Int32 v) {
			byte []buf = BitConverter.GetBytes (v);
			ms.Write (buf, 0, buf.Length);
		}
        
        /// <summary>
        /// Encode BSON Int64
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="v"></param>
		private void EncodeInt64(MemoryStream ms, Int64 v) {
			byte []buf = BitConverter.GetBytes (v);
			ms.Write (buf, 0, buf.Length);
		}
        
        /// <summary>
        /// Encode BSON Utc DateTime
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="dt"></param>
		private void EncodeUtcDateTime(MemoryStream ms, DateTime dt) {
			TimeSpan span;
			if(dt.Kind == DateTimeKind.Local) {
				span = (dt - new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime());				
			}
			else {
				span = dt - new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);				
			}			
			byte []buf = BitConverter.GetBytes ((Int64)(span.TotalSeconds * 1000));
			ms.Write (buf, 0, buf.Length);
		}
    }
}