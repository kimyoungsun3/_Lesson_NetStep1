using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		public static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);
		
		//키보드 이벤트 
		[DllImport("user32.dll")]
		public static extern void keybd_event(
			byte bVk,  //virtual-key code
			byte bScan, //hardware scan code
			int dwFlags,  //function options 
			ref int dwExtraInfo  //additional keystroke data 
			);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		private bool _start;
		private void Start()
		{
			_start = true;
			IntPtr _pointer = FindWindow(new IntPtr(0), textBox1.Text);
			SetForegroundWindow(_pointer);
			int _sleep = 1;
			while (_start)
			{
				//RefreshSendKey();
				_sleep = (int)((float)numericUpDown1.Value * 1000);
				if (_sleep < 250) _sleep = 250;
				CtrlZ();
				//Thread.Sleep(_sleep * 1000);
				Application.DoEvents();
				Thread.Sleep(_sleep);
			}
		}

		private void RefreshSendKey()
		{
			int key = int.Parse(textBox2.Text);
			const int KEYEVENTF_KEYUP = 0x0002;
			int kkk = 0;
			keybd_event((byte)key, 0, 0, ref kkk);
			keybd_event((byte)key, 0, KEYEVENTF_KEYUP, ref kkk);
		}

		private void CtrlZ()
		{
			int keyCtrl		= 17;
			int keyZ		= 90;
			const int KEY_DOWN = 0x0000;
			const int KEY_UP = 0x0002;
			int kkk = 0;
			keybd_event((byte)keyCtrl, 0, KEY_DOWN, ref kkk);
			keybd_event((byte)keyZ, 0, KEY_DOWN, ref kkk);
			keybd_event((byte)keyZ, 0, KEY_UP, ref kkk);
			keybd_event((byte)keyCtrl, 0, KEY_UP, ref kkk);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Start();
			//SendKeys.Send("{F5}");
			//SendKeys.Send("(^z)");
			//timer1.Enabled = true;
			//IntPtr WindowHandle = FindWindow(textBox1.Text, textBox1.Text);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			_start = false;
			//SendKeys.Send("^z");
			////timer1.Enabled = false;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			//SendKeys.Send(textBox1.Text);
			////SendKeys.Send("1234");
			//SendKeys.Send("(^z)");
			////SendKeys.Send("{Enter}");
			////SendKeys.Send
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{

		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
	}
}
