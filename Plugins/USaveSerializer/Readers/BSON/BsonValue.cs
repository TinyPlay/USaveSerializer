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

namespace USaveLib.Readers.BSON
{
    /// <summary>
    /// BSON Value Class
    /// </summary>
    public class BsonValue
    {
	    /// <summary>
	    /// BSON Value Type Enum
	    /// </summary>
        public enum ValueType {
			Double,
			String,
			Array,
			Binary,
			Boolean,
			UtcDateTime,
			None,
			Int32,
			Int64,
			Object
		};

	    // BSON Value Types
		private ValueType _mValueType;
		private Double _double;
		private string _string;
		private byte[] _binary;
		private bool _bool;
		private DateTime _dateTime;
		private Int32 _int32;
		private Int64 _int64;

		#region BSON Properties
		public ValueType valueType { get { return _mValueType; } }
		public Double doubleValue {
			get {
				switch (_mValueType) {
					case ValueType.Int32:
					return (double)_int32;
					case ValueType.Int64:
					return (double)_int64;
					case ValueType.Double:
					return _double;
					case ValueType.None:
					return float.NaN;
				}
				
				throw new Exception(string.Format("Original type is {0}. Cannot convert from {0} to double", _mValueType));
			}
		}
		public Int32 int32Value {
			get {
				switch (_mValueType) {
					case ValueType.Int32:
					return (Int32)_int32;
					case ValueType.Int64:
					return (Int32)_int64;
					case ValueType.Double:
					return (Int32)_double;
				}

				throw new Exception(string.Format("Original type is {0}. Cannot convert from {0} to Int32", _mValueType));
			}
		}
		public Int64 int64Value {
			get {
				switch (_mValueType) {
					case ValueType.Int32:
					return (Int64)_int32;
					case ValueType.Int64:
					return (Int64)_int64;
					case ValueType.Double:
					return (Int64)_double;
				}

				throw new Exception(string.Format("Original type is {0}. Cannot convert from {0} to Int64", _mValueType));
			}
		}
		public byte []binaryValue {
			get {
				switch (_mValueType) {
					case ValueType.Binary:
					return _binary;
				}

				throw new Exception(string.Format("Original type is {0}. Cannot convert from {0} to binary", _mValueType));
			}
		}
		public DateTime dateTimeValue {
			get {
				switch (_mValueType) {
					case ValueType.UtcDateTime:
					return _dateTime;
				}
				
				throw new Exception(string.Format("Original type is {0}. Cannot convert from {0} to DateTime", _mValueType));
			}
		}
		public String stringValue {
			get {
				switch (_mValueType) {
					case ValueType.Int32:
					return Convert.ToString (_int32);
					case ValueType.Int64:
					return Convert.ToString (_int64);
					case ValueType.Double:
					return Convert.ToString (_double);
					case ValueType.String:
					return _string != null ? _string.TrimEnd(new char[] {(char)0} ) : null;
					case ValueType.Boolean:
					return _bool == true ? "true" : "false";
					case ValueType.Binary:
					return Encoding.UTF8.GetString(_binary).TrimEnd(new char[] {(char)0} );
				}

				throw new Exception(string.Format("Original type is {0}. Cannot convert from {0} to string", _mValueType));
			}
		}
		public bool boolValue {
			get {
				switch (_mValueType) {
					case ValueType.Boolean:
					return _bool;
				}

				throw new Exception(string.Format("Original type is {0}. Cannot convert from {0} to bool", _mValueType));
			}
		}
		public bool isNone {
			get { return _mValueType == ValueType.None; }
		}
		
		public virtual BsonValue this [string key] => null;
		public virtual BsonValue this [int index]
		{
			get => null;
		}
		#endregion

		#region BSON Operators
		public virtual void Clear() {}
		public virtual void Add (string key, BsonValue value) {}
		public virtual void Add (BsonValue value) {}
		public virtual bool Contains(BsonValue v) { return false; }
		public virtual bool ContainsKey(string key) { return false; }

		public static implicit operator BsonValue(double v) {
			return new BsonValue (v);
		}

		public static implicit operator BsonValue(Int32 v) {
			return new BsonValue (v);
		}

		public static implicit operator BsonValue(Int64 v) {
			return new BsonValue (v);
		}

		public static implicit operator BsonValue(byte []v) {
			return new BsonValue (v);
		}

		public static implicit operator BsonValue(DateTime v) {
			return new BsonValue (v);
		}

		public static implicit operator BsonValue(string v) {
			return new BsonValue (v);
		}

		public static implicit operator BsonValue(bool v) {
			return new BsonValue (v);
		}
		

		public static implicit operator double(BsonValue v) {
			return v.doubleValue;
		}

		public static implicit operator Int32(BsonValue v) {
			return v.int32Value;
		}
		
		public static implicit operator Int64(BsonValue v) {
			return v.int64Value;
		}

		public static implicit operator byte [](BsonValue v) {
			return v.binaryValue;
		}

		public static implicit operator DateTime(BsonValue v) {
			return v.dateTimeValue;
		}

		public static implicit operator string(BsonValue v) {
			return v.stringValue;
		}

		public static implicit operator bool(BsonValue v) {
			return v.boolValue;
		}
		#endregion

		#region BSON Value Constructors
		protected BsonValue (ValueType valueType)
		{
			_mValueType = valueType;
		}
		public BsonValue() {
			_mValueType = ValueType.None;
		}
		public BsonValue(double v) {
			_mValueType = ValueType.Double;
			_double = v;
		}
		public BsonValue(String v) {
			_mValueType = ValueType.String;
			_string = v;
		}
		public BsonValue(byte [] v) {
			_mValueType = ValueType.Binary;
			_binary = v;
		}
		public BsonValue(bool v) {
			_mValueType = ValueType.Boolean;
			_bool = v;
		}
		public BsonValue(DateTime dt) {
			_mValueType = ValueType.UtcDateTime;
			_dateTime = dt;
		}
		public BsonValue(Int32 v) {
			_mValueType = ValueType.Int32;
			_int32 = v;
		}
		public BsonValue(Int64 v) {
			_mValueType = ValueType.Int64;
			_int64 = v;
		}
		#endregion

		#region BSON Compare Operators
		public static bool operator ==(BsonValue a, object b)
		{
			return System.Object.ReferenceEquals (a, b);
		}
		public static bool operator !=(BsonValue a, object b)
		{
			return !(a == b);
		}
		#endregion
    }
}