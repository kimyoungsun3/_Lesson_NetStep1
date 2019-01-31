using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;

public class Client : MonoBehaviour {
	int port = 10045;
	byte[] receBuf = new byte[1024*4];
	byte[] sendBuf = new byte[1024*4];
	Socket clientSocket = new Socket (
		AddressFamily.InterNetwork,
		SocketType.Stream,
		ProtocolType.Tcp);

	void Start () {
		Debug.Log ("Client Start");
		StartCoroutine( Connecting ());
	}

	IEnumerator Connecting(){
		Debug.Log ("Client try Connecting()");
		int _count = 0;
		while (!clientSocket.Connected) {
			try{
				_count++;
				clientSocket.Connect (IPAddress.Loopback, port);
			}catch(SocketException){
				Debug.Log ("Connection attempts:" + _count);
			}
			yield return new WaitForSeconds(0.1f);
		}

		Debug.Log ("Client Connecting -> OK");
	}

	string str;
	bool bClick = false;
	void Update () {
		Debug.Log ("----");
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			Debug.Log (11);
			bClick = true;
			str = "get time";
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			Debug.Log (12);
			bClick = true;
			str = "xxxxxx";			
		}

		if (bClick) {
			Debug.Log (121);
			Debug.Log ("Client send");
			bClick = false;

			Debug.Log (122);
			byte[] _buffer = Encoding.ASCII.GetBytes (str);
			Debug.Log(_buffer.Length);
			clientSocket.Send (_buffer);

			Debug.Log (123);
			int _size = clientSocket.Receive (receBuf);
			//byte[] _data = new byte[_size];
			//Array.Copy (receBuf, _data, _size);
			Debug.Log ("Received:[" + Encoding.ASCII.GetString (receBuf, 0, _size)+"]");
		}
	}
}
