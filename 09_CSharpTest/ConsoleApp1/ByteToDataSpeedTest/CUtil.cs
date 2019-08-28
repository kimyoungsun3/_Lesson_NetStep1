using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteToDataSpeedTest
{
	public static class CUtil
	{
		//--------------------------------------
		// byte
		// [] -> data
		//--------------------------------------
		//GetByte(); 직접

		////--------------------------------------
		// short(Int16)
		// [] <-> data
		//	SetShort:	113	104	103	102
		//	SetShort2:	627	620	620	621
		//	GetShort:	105	105	103	105
		//	GetShort2:	121	116	117	117
		//--------------------------------------
		public static void SetShort(byte[] _target, int _pos, short _val)
		{
			_target[_pos + 1] = (byte)((_val >> 8) & 0xFF);
			_target[_pos + 0] = (byte)((_val >> 0) & 0xFF);
		}

		public static void SetShort2(byte[] _target, int _pos, short _val)
		{
			//short -> byte[4] -> Array.Copy
			byte[] _arr = BitConverter.GetBytes(_val);
			Array.Copy(_arr, 0, _target, _pos, _arr.Length);
		}

		public static short GetShort(byte[] _src, int _pos)
		{
			return (short)(
				   _src[_pos + 0]
				| (_src[_pos + 1] << 8));
		}

		public static short GetShort2(byte[] _src, int _pos)
		{
			return BitConverter.ToInt16(_src, _pos);
		}

		//--------------------------------------
		// int(Int32)
		// [] <-> data
		//	SetInt	:	152	151	150	151
		//	SetInt2	:	642	631	634	637
		//	GetInt	:	154	152	157	152
		//	GetInt2	:	125 124	122	121
		//--------------------------------------
		public static void SetInt(byte[] _target, int _pos, int _val)
		{
			_target[_pos + 3] = (byte)((_val >> 24) & 0xFF);
			_target[_pos + 2] = (byte)((_val >> 16) & 0xFF);
			_target[_pos + 1] = (byte)((_val >>  8) & 0xFF);
			_target[_pos + 0] = (byte)((_val >>  0) & 0xFF);
		}

		public static void SetInt2(byte[] _target, int _pos, int _val)
		{
			//int -> byte[4] -> Array.Copy
			byte[] _arr = BitConverter.GetBytes(_val);
			Array.Copy(_arr, 0, _target, _pos, _arr.Length);
		}

		public static int GetInt(byte[] _src, int _pos)
		{
			return (int)(
				   _src[_pos + 0]
				| (_src[_pos + 1] <<  8)
				| (_src[_pos + 2] << 16)
				| (_src[_pos + 3] << 24));
		}

		public static int GetInt2(byte[] _src, int _pos)
		{
			return BitConverter.ToInt32(_src, _pos);
		}

		//--------------------------------------
		// float는 2개가 성능 차이를 못낸다.
		//SetFloat	:	658	647	653	648	653
		//SetFloat2 :	658	647	653	648	653
		//GetFloat	:	139	138	138	138	137
		//GetFloat2	:	139	138	138	138	137
		//--------------------------------------
		public static void SetFloat(byte[] _target, int _pos, float _val)
		{
			//float -> byte[4] -> Array.Copy
			byte[] _arr = BitConverter.GetBytes(_val);
			Array.Copy(_arr, 0, _target, _pos, _arr.Length);
		}

		////컴파일 옵션을 unsafe로 해둬야한다.
		//public static unsafe void SetFloat2(byte[] _target, int _pos, float _val)
		//{
		//	uint _val2 = *((uint*)&_val);
		//	_target[_pos + 3] = (byte)((_val2 >> 24) & 0xFF);
		//	_target[_pos + 2] = (byte)((_val2 >> 16) & 0xFF);
		//	_target[_pos + 1] = (byte)((_val2 >> 8) & 0xFF);
		//	_target[_pos + 0] = (byte)((_val2 >> 0) & 0xFF);
		//}

		public static float GetFloat(byte[] _src, int _pos)
		{
			return (float)BitConverter.ToSingle(_src, _pos);
		}
		
		//public static unsafe float GetFloat2(byte[] _src, int _pos)
		//{
		//	uint i = (uint)(
		//		  _src[_pos + 0]
		//	   | (_src[_pos + 1] << 8)
		//	   | (_src[_pos + 2] << 16)
		//	   | (_src[_pos + 3] << 24));
		//	return *(((float*)&i));
		//}

		//--------------------------------------
		//SetDoublie	:	
		//GetDoublie	:	
		//--------------------------------------
		public static void SetDouble(byte[] _target, int _pos, double _val)
		{
			//float -> byte[4] -> Array.Copy
			byte[] _arr = BitConverter.GetBytes(_val);
			Array.Copy(_arr, 0, _target, _pos, _arr.Length);
		}

		public static double GetDouble(byte[] _src, int _pos)
		{
			return (double)BitConverter.ToDouble(_src, _pos);
		}

		//--------------------------------------
		//사이즈(2) + 문자열(n)
		//SetString	:	262	267	255	279
		//GetString	:	834	843	814	832
		//--------------------------------------
		public static void SetString(byte[] _target, int _pos, string _val)
		{
			byte[] _body = System.Text.Encoding.UTF8.GetBytes(_val);
			byte[] _header = BitConverter.GetBytes((Int16)_body.Length);

			//사이즈(2) + 문자열(n)
			Array.Copy(_header, 0, _target, _pos, _header.Length);
			_pos += 2;// _header.Length;

			Array.Copy(_body, 0, _target, _pos, _body.Length);
			//_pos += _body.Length;			
		}

		public static void SetString2(byte[] _target, int _pos, ref string _val)
		{
			byte[] _body = System.Text.Encoding.UTF8.GetBytes(_val);
			byte[] _header = BitConverter.GetBytes((Int16)_body.Length);

			//사이즈(2) + 문자열(n)
			Array.Copy(_header, 0, _target, _pos, _header.Length);
			_pos += _header.Length;

			Array.Copy(_body, 0, _target, _pos, _body.Length);
			//_pos += _body.Length;			
		}

		public static string GetString(byte[] _src, int _pos)
		{
			Int16 _len = BitConverter.ToInt16(_src, _pos);
			_pos += 2;//sizeof(Int16);

			// 인코딩은 utf8로 통일한다.
			string _str = System.Text.Encoding.UTF8.GetString(_src, _pos, _len);
			//_pos += _len;

			return _str;
		}

		public static void GetString2(byte[] _src, int _pos, out string _str)
		{
			Int16 _len = BitConverter.ToInt16(_src, _pos);
			_pos += 2;

			// 인코딩은 utf8로 통일한다.
			_str = System.Text.Encoding.UTF8.GetString(_src, _pos, _len);
			//_pos += _len;

			//return _str;
		}

	}
}
