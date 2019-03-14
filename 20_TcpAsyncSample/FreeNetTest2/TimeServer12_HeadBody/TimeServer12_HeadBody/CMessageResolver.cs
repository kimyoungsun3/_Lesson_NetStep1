using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TimeServer12_HeadBody
{
	public class CMessageResolver
	{
		byte[] messageBuffer = new byte[1024];
		int remainBytes;
		int messageSize;
		int messageOffset;
		int messageToRead;
		public delegate void CompletedMessageCallback(byte[] _buffer, int _size);

		public CMessageResolver()
		{
			remainBytes = 0;
			messageSize = 0;
			messageOffset = 0;
			messageToRead = 0;
		}

		public void ReadReceive(byte[] _buffer, int _offset, int _transferred, CompletedMessageCallback _onParseCode)
		{
			if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine(this + " ReadReceive\r\n _buffer:{0} _offset:{1} _transferred:{2}\r\n _callback(받은메세지를 이콜백으로 처리함)", _buffer, _offset, _transferred);
			remainBytes = _transferred;
			int _offset2 = _offset;
			bool _completed = false;

			while (remainBytes > 0)
			{
				if (Protocol.DEBUG_PACKET_RECEIVE)
					Console.WriteLine(" > _offset:{0} _offset2:{1} _transferred:{2}\r\n"
										+ " > messageOffset:{3} remainBytes:{4} messageToRead:{5} messageSize:{6}",
										_offset, _offset2, _transferred,
										messageOffset, remainBytes, messageToRead, messageSize);
				_completed = false;

				if (messageOffset < Protocol.HEADER_SIZE)
				{
					messageToRead = Protocol.HEADER_SIZE;
					if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("   > (헤더읽기) messageOffset:{0} messageToRead:{1}", messageOffset, messageToRead);
					_completed = ReadUtil(_buffer, ref _offset2, _offset, _transferred);
					if (!_completed)
					{
						if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("    > (**** 헤더아직 안들어와서 리턴 *****");
						return;
					}

					// 헤더 하나를 온전히 읽어왔으므로 메시지 사이즈를 구한다.
					messageSize = Util.GetShort(messageBuffer, 0);
					if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("   > 해더만읽고 messageSize:{0}", messageSize);

					// 다음 목표 지점(헤더 + 메시지 사이즈).
					messageToRead = Protocol.HEADER_SIZE + messageSize;
					if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("   > messageSize:{0} messageToRead:{1}", messageSize, messageToRead);
				}

				// 메시지를 읽는다.
				_completed = ReadUtil(_buffer, ref _offset2, _offset, _transferred);

				if (_completed)
				{
					if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("   > 메세지를 정상읽음 > 메세지 처리하러가기~~~");
					// 패킷 하나를 완성 했다.
					_onParseCode(messageBuffer, messageSize);

					ClearBuffer();
				}
				else
				{
					if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("   > 메세지를 비정상 ****> 자동대기중~~~");
				}
			}
		}

		bool ReadUtil(byte[] _buffer, ref int _offset2, int _offset, int _transferred)
		{
			bool _rtn = false;
			if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine(this + " ReadUtil\r\n _buffer:{0}, ref _offset2:{1} _offset:{2} _transferred:{3}", _buffer, _offset2, _offset, _transferred);
			if(_offset2 >= _offset + _transferred)
			{
				// 들어온 데이터 만큼 다 읽은 상태이므로 더이상 읽을 데이터가 없다.
				if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("  > 들어온 데이타 사이즈만큼 다읽음");
				return _rtn;
			}

			// 읽어와야 할 바이트.
			// 데이터가 분리되어 올 경우 이전에 읽어놓은 값을 빼줘서 부족한 만큼 읽어올 수 있도록 계산해 준다.
			int _copySize = messageToRead - messageOffset;
			if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("  > _copySize:{0} = messageToRead:{1} - messageOffset:{2}", _copySize, messageToRead, messageOffset);
			if(remainBytes < _copySize)
			{
				_copySize = remainBytes;
			}
			Array.Copy(_buffer, _offset2, messageBuffer, messageOffset, _copySize);
			_offset2		+= _copySize;
			messageOffset	+= _copySize;
			remainBytes		-= _copySize;

			if(messageOffset < messageToRead)
			{
				//해더에 1byte만 올경우.
				if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("   > 지정된곳까지 남음");
				_rtn = false;
			}
			else
			{
				if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine("   > 지정된곳까지 다읽음");
				_rtn = true;
			}

			return _rtn;
		}

		void ClearBuffer()
		{
			if (Protocol.DEBUG_PACKET_RECEIVE) Console.WriteLine(this + " ClearBuffer");
			messageOffset = 0;
			messageSize = 0;
		}
	}
}
