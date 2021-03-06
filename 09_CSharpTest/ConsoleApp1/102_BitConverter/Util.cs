﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _102_BitConverter
{

	public static class Util
	{
		//--------------------------------------
		//읽기..
		//--------------------------------------
		//GetByte(); 직접

		public static short GetShort(byte[] _src, int _pos)
		{
			return (short)(
				   _src[_pos + 0]
				| (_src[_pos + 1] << 8));
		}

		public static int GetInt(byte[] _src, int _pos)
		{
			return (int)(
				   _src[_pos + 0]
				| (_src[_pos + 1] <<  8)
				| (_src[_pos + 2] << 16)
				| (_src[_pos + 3] << 24));
		}

		public static float GetFloat(byte[] _src, int _pos)
		{
			return (short)BitConverter.ToSingle(_src, _pos);
		}

		// 위에것 보다 느림...
		public static short GetShort2(byte[] _src, int _pos)
		{
			return (short)BitConverter.ToInt16(_src, _pos);
		}
		public static int GetInt2(byte[] _src, int _pos)
		{
			return BitConverter.ToInt32(_src, _pos);
		}

		public static void SetShort(byte[] _target, int _pos, short _val)
		{
			//byte[] _arr = BitConverter.GetByte(short num);
			_target[_pos + 1] = (byte)((_val >> 8) & 0xFF);
			_target[_pos + 0] = (byte)((_val >> 0) & 0xFF);
		}

		public static void SetInt(byte[] _target, int _pos, int _val)
		{
			//int -> byte[4] -> copy, 임시공간.
			//byte[] _arr = BitConverter.GetByte(int num);

			//다이렉트 입력..
			_target[_pos + 3] = (byte)((_val >> 24) & 0xFF);
			_target[_pos + 2] = (byte)((_val >> 16) & 0xFF);
			_target[_pos + 1] = (byte)((_val >>  8) & 0xFF);
			_target[_pos + 0] = (byte)((_val >>  0) & 0xFF);
		}
		//byte[] Array.Copy(src, 0, target, 2, 4);

	}
}
