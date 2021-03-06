﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;


using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

using SocketGlobal;

namespace SocketServer
{

	#region 유저에게 연결할 델리게이트
	//델리게이트 선언
	/// <summary>
	/// 유저 접속
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void dgConnect(claUser sender);
	/// <summary>
	/// 유저 접속 끊김
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void dgDisconnect(claUser sender);
	/// <summary>
	/// 유저 메시지 요청
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void dgMessage(claUser sender, MessageEventArgs e);
	#endregion

	public partial class frmServer : Form
	{
		/// <summary>
		/// 접속한 유저 리스트(로그인 완료전 포함)
		/// </summary>
		private List<claUser> m_listUser = null;

		/// <summary>
		/// 서버 소켓
		/// </summary>
		private Socket socketServer;

		public frmServer()
		{
			InitializeComponent();

			BtnDisplay(true);
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			//서버 세팅
			socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint ipepServer = new IPEndPoint(IPAddress.Any, Convert.ToInt32(txtPort.Text));
			socketServer.Bind(ipepServer);
			socketServer.Listen(20);

			SocketAsyncEventArgs saeaUser = new SocketAsyncEventArgs();
			//유저가 연결되었을때 이벤트
			saeaUser.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);
			//유저 접속 대기 시작
			socketServer.AcceptAsync(saeaUser);

			//유저 리스트 생성
			m_listUser = new List<claUser>();
			//서버 시작 로그 표시
			DisplayLog("서버 시작");
			//버튼 표시
			BtnDisplay(false);
		}

		/// <summary>
		/// 버튼을 화면에 표시하거나 가린다.
		/// </summary>
		/// <param name="bView"></param>
		private void BtnDisplay(bool bView)
		{
			if (true == bView)
			{
				btnStart.Enabled = true;
				btnStop.Enabled = false;
			}
			else
			{
				btnStart.Enabled = false;
				btnStop.Enabled = true;
			}

		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			//서버 중지
			socketServer.Close();
			BtnDisplay(true);
		}

		/// <summary>
		/// 클라이언트가 연결되면 발생
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Accept_Completed(object sender, SocketAsyncEventArgs e)
		{
			//클라이언트 접속

			//유저 객체를 만든다.
			claUser insUser = new claUser(e.AcceptSocket);
			//각 이벤트 연결
			insUser.OnConnected += insUser_OnConnected;
			insUser.OnDisconnected += insUser_OnDisconnected;
			insUser.OnMessaged += insUser_OnMessaged;

			//리스트에 클라이언트 추가
			m_listUser.Add(insUser);
			//서버에 접속자 리스트 표시

			DisplayLog(" --- 유저 접속 시작 --- ");

			//다시 클라이언트 접속을 기다린다.
			Socket socketServer = (Socket)sender;
			e.AcceptSocket = null;
			socketServer.AcceptAsync(e);
		}

		/// <summary>
		/// 유저 접속 이벤트
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void insUser_OnConnected(claUser sender)
		{
			//유저 추가는 'Accept_Completed'에서 하므로 
			//여기서 하는 것은 무결성 검사가 끝난 유저를 처리 해주는 것이다.

			StringBuilder sbMsg = new StringBuilder();;

			//로그인이 완료된 유저에게 유저 리스트를 보낸다.
			Commd_User_List_Get(sender);

			//전체 유저에게 접속자를 알린다.
			sbMsg.Clear();
			sbMsg.Append(claCommand.Command.User_Connect.GetHashCode());
			sbMsg.Append(claGlobal.g_Division);
			sbMsg.Append(sender.UserID);

			//전체 유저에게 메시지 전송(지금 로그인 한 접속자는 제외)
			AllUser_Send(sbMsg.ToString(), sender);

			//로그 유저 리스트에 추가
			this.Invoke(new Action(
				delegate()
				{
					listUser.Items.Add(sender.UserID);
				}));

			//로그 남기기
			sbMsg.Clear();
			sbMsg.Append("*** 접속자 : ");
			sbMsg.Append(sender.UserID);
			sbMsg.Append(" ***");
			DisplayLog(sbMsg.ToString());

		}

		/// <summary>
		/// 유저 끊김 이벤트
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void insUser_OnDisconnected(claUser sender)
		{
			StringBuilder sbMsg = new StringBuilder();
			sbMsg.Append(" *** ");
			sbMsg.Append(sender.UserID);
			sbMsg.Append(" : 접속 끊김 *** ");

			//로그리스트에서 유저를 지움
			//출력
			this.Invoke(new Action(
						delegate()
						{
							listUser.Items.RemoveAt(listUser.FindString(sender.UserID));
						}));

			//로그 기록
			DisplayLog(sbMsg.ToString());
			//리스트에서 유저를 지움
			m_listUser.Remove(sender);

			//전체 유저에게 메시지 전송
			AllUser_Send(sbMsg.ToString());
		}

		/// <summary>
		/// 유저 메시지 이벤트
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void insUser_OnMessaged(claUser sender, MessageEventArgs e)
		{
			StringBuilder sbMsg = new StringBuilder();
			

			switch (e.m_typeCommand)
			{
				case claCommand.Command.Msg:	//메시지
					sbMsg.Append(sender.UserID );
					sbMsg.Append(" : ");
					sbMsg.Append(e.m_strMsg);

					Commd_SendMsg(sbMsg.ToString());
					break;
				case claCommand.Command.ID_Check:	//id체크
					Commd_IDCheck(sender, e.m_strMsg);
					break;
				case claCommand.Command.User_List_Get:	//유저 리스트 갱신 요청
					Commd_User_List_Get(sender);
					break;
			}
			
		}

		/// <summary>
		/// 명령 처리 - 메시지 보내기
		/// </summary>
		/// <param name="sMsg"></param>
		private void Commd_SendMsg(string sMsg)
		{
			StringBuilder sbMsg = new StringBuilder();
			//명령어 부착
			sbMsg.Append(claCommand.Command.Msg.GetHashCode().ToString());
			//구분자 부착
			sbMsg.Append(claGlobal.g_Division);
			//메시지 완성
			sbMsg.Append(sMsg);

			//전체 유저에게 메시지 전송
			AllUser_Send(sbMsg.ToString());
			
		}

		/// <summary>
		/// 명령 처리 - ID체크
		/// </summary>
		/// <param name="sID"></param>
		private void Commd_IDCheck(claUser insUser, string sID)
		{
			//사용 가능 여부
			bool bReturn = true;

			//모든 유저의 아이디 체크
			foreach (claUser insUserTemp in m_listUser)
			{
				if (insUserTemp.UserID == sID)
				{
					//같은 유저가 있다!
					//같은 유저가 있으면 그만 검사한다.
					bReturn = false;
					break;
				}
			}

			if (true == bReturn)
			{
				//사용 가능

				//아이디를 지정하고
				insUser.UserID = sID;

				//유저에게 로그인이 성공했음을 알림
				StringBuilder sbMsg = new StringBuilder();
				//접속자에게 먼저 로그인이 성공했음을 알린다.
				sbMsg.Append(claCommand.Command.ID_Check_Ok.GetHashCode());
				sbMsg.Append(claGlobal.g_Division);
				insUser.SendMsg_User(sbMsg.ToString());

				//유저가 접속 했음을 직접 알리지 말고 'ID_Check_Ok'를 받은
				//클라이언트가 직접 요청한다.
			}
			else
			{
				//검사 실패를 알린다.

				StringBuilder sbMsg = new StringBuilder();

				sbMsg.Append(claCommand.Command.ID_Check_Fail.GetHashCode().ToString());
				sbMsg.Append(claGlobal.g_Division);

				insUser.SendMsg_User(sbMsg.ToString());
			}
		}

		/// <summary>
		/// 명령 처리 - 유저 리스트 갱신 요청
		/// </summary>
		/// <param name="insUser"></param>
		private void Commd_User_List_Get(claUser insUser)
		{
			StringBuilder sbList = new StringBuilder();

			//명령 만들기
			sbList.Append(claCommand.Command.User_List.GetHashCode());
			sbList.Append(claGlobal.g_Division);

			//리스트 만들기
			foreach (claUser insUser_Temp in m_listUser)
			{
				sbList.Append(insUser_Temp.UserID);
				sbList.Append(",");
			}

			//요청에 응답해준다.
			insUser.SendMsg_User(sbList.ToString());
		}

		/// <summary>
		/// 접속중인 모든 유저에게 메시지를 보냅니다.
		/// </summary>
		/// <param name="sMsg"></param>
		private void AllUser_Send(string sMsg)
		{
			//모든 유저에게 메시지를 전송 한다.
			foreach (claUser insUser in m_listUser)
			{
				insUser.SendMsg_User(sMsg);
			}

			//로그 출력
			DisplayLog(sMsg);
		}

		/// <summary>
		/// 전체 유저중 지정한 유저를 제외하고 메시지를 전송 합니다.
		/// </summary>
		/// <param name="sMsg"></param>
		/// <param name="insUser">제외할 유저</param>
		private void AllUser_Send(string sMsg, claUser insUser)
		{
			//모든 유저에게 메시지를 전송 한다.
			foreach (claUser insUser_Temp in m_listUser)
			{
				//제외 유저
				if (insUser_Temp.UserID != insUser.UserID)
				{
					//제외 유저가 아니라면 메시지를 보낸다.
					insUser_Temp.SendMsg_User(sMsg);
				}
			}

			//로그 출력
			DisplayLog(sMsg);
		}

		/// <summary>
		/// 받아온 메시지를 출력 한다.
		/// </summary>
		/// <param name="nMessage"></param>
		/// <param name="nType"></param>
		public void DisplayLog(String nMessage)
		{
			StringBuilder buffer = new StringBuilder();
			
			//출력할 메시지 완성
			buffer.Append(nMessage);

			//출력
			this.Invoke(new Action(
						delegate()
						{
							listLog.Items.Add(nMessage);
						}));
			
		}

		private void listLog_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
