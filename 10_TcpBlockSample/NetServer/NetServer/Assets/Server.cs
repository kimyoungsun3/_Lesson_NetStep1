using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;

public class Server : MonoBehaviour {
	
	public List<Socket> clients = new List<Socket>();

	int port = 10045;
	byte[] receBuf = new byte[1024*4];
	byte[] sendBuf = new byte[1024*4];
	List<Socket> clientSockets = new List<Socket> ();
	Socket serverSocket = new Socket (
		                      AddressFamily.InterNetwork,
		                      SocketType.Stream,
		                      ProtocolType.Tcp);

	void Start () {
		Debug.Log ("Server Start()");
		SetupServer ();
	}

	void SetupServer(){
		Debug.Log ("SetupServer()");
		serverSocket.Bind (new IPEndPoint (IPAddress.Any, port));
		Debug.Log (11);
		serverSocket.Listen (100);
		Debug.Log (12);
		serverSocket.BeginAccept (new AsyncCallback (AcceptCallBack), null);
		Debug.Log (13);
	}

	void AcceptCallBack(IAsyncResult _ar){
		Debug.Log ("AcceptCallBack(IAsyncResult _ar)");
		Socket _socket = serverSocket.EndAccept (_ar);
		clients.Add(_socket);
		Debug.Log (21);
		clientSockets.Add (_socket);
		Debug.Log (22);
		_socket.BeginReceive (receBuf, 0, receBuf.Length, SocketFlags.None, new AsyncCallback (ReceiveCallBack), null);
		Debug.Log (23);
		serverSocket.BeginAccept (new AsyncCallback (AcceptCallBack), null);
		Debug.Log (24);
		Debug.Log("Client count:" + clients.Count);

	}

	void ReceiveCallBack(IAsyncResult _ar){
		Debug.Log (" ReceiveCallBack(IAsyncResult _ar)");
		Debug.Log (311 +":" + _ar);
		string[] _msg = (string[])_ar.AsyncState;
		Debug.Log (312 + ":" + _msg);
		//if(_socket == null)
		//{
		//	Debug.Log(" > Client Distconnect");
		//	return;
		//}

		Socket _socket = clients[0];
		int _receSize = _socket.EndReceive (_ar);
		Debug.Log (313);
		byte[] _tempBuf = new byte[_receSize];
		Debug.Log (314);
		Array.Copy (receBuf, _tempBuf, _receSize);
		Debug.Log (315);

		string _text = Encoding.ASCII.GetString (_tempBuf);
		Debug.Log ("Text received:" + _text);
		Debug.Log (32);

		if (_text.ToLower ().Equals ("get time")) {
			_text = DateTime.Now.ToLongTimeString ();
		} else {
			_text = "Invalied Request";
		}
		Debug.Log (33);

		byte[] _data = Encoding.ASCII.GetBytes (_text);
		_socket.BeginSend(_data, 0, _data.Length, SocketFlags.None, new AsyncCallback (SendCallBack), null);

		Debug.Log (34);
	}

	void SendCallBack(IAsyncResult _ar){
		Debug.Log (" SendCallBack(IAsyncResult _ar)");
		Socket _socket = (Socket)_ar.AsyncState;
		Debug.Log (41);
		_socket.EndSend (_ar);
		Debug.Log (42);
	}
}
