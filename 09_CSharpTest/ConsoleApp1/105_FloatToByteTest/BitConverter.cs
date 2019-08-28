
using System;
using System.Diagnostics;

namespace netduino
{
	public static class BitConverter
	{
		public static unsafe void SetFloat(byte[] _target, int _pos, float _val)
		{
			uint _val2 = *((uint*)&_val);

			_target[_pos + 3] = (byte)((_val2 >> 24) & 0xFF);
			_target[_pos + 2] = (byte)((_val2 >> 16) & 0xFF);
			_target[_pos + 1] = (byte)((_val2 >> 8)  & 0xFF);
			_target[_pos + 0] = (byte)((_val2 >> 0)  & 0xFF);
		}

		public static unsafe float GetFloat(byte[] _src, int _pos)
		{
			uint i = (uint)(
				  _src[_pos + 0]
			   | (_src[_pos + 1] << 8)
			   | (_src[_pos + 2] << 16)
			   | (_src[_pos + 3] << 24));

			return *(((float*)&i));
		}

		//---------------------------------------------------
		public static byte[] GetBytes(uint value)
		{
			return new byte[4] {
					(byte)( value        & 0xFF),
					(byte)((value >>  8) & 0xFF),
					(byte)((value >> 16) & 0xFF),
					(byte)((value >> 24) & 0xFF) };
		}

		public static unsafe byte[] GetBytes(float value)
		{
			uint val = *((uint*)&value);
			return GetBytes(val);
		}

		public static unsafe byte[] GetBytes(float value, ByteOrder order)
		{
			byte[] bytes = GetBytes(value);
			if (order != ByteOrder.LittleEndian)
			{
				System.Array.Reverse(bytes);
			}
			return bytes;
		}

		public static uint ToUInt32(byte[] value, int index)
		{
			return (uint)(
				value[0 + index] << 0 |
				value[1 + index] << 8 |
				value[2 + index] << 16 |
				value[3 + index] << 24);
		}

		public static unsafe float ToSingle(byte[] value, int index)
		{
			uint i = ToUInt32(value, index);
			return *(((float*)&i));
		}

		public static unsafe float ToSingle(byte[] value, int index, ByteOrder order)
		{
			if (order != ByteOrder.LittleEndian)
			{
				System.Array.Reverse(value, index, value.Length);
			}
			return ToSingle(value, index);
		}

		public enum ByteOrder
		{
			LittleEndian,
			BigEndian
		}

		static public bool IsLittleEndian
		{
			get
			{
				unsafe
				{
					int i = 1;
					char* p = (char*)&i;

					return (p[0] == 1);
				}
			}
		}
	}
}
