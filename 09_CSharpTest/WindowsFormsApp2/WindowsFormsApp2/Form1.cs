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


namespace WindowsFormsApp2
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		[DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, ref int dwExtraInfo );

		private void button1_Click(object sender, EventArgs e)
		{
			Start();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			bStart = false;
		}

		private bool bStart;
		private void Start()
		{
			bStart = true;
			int _sleep = 1;
			int _count	= 0;
			int _MAX	= 0;
			while (bStart)
			{
				_sleep	= (int)(float.Parse(textBox1.Text) * 1000);
				_MAX	= int.Parse(textBox2.Text);
				_count++;
				display.Text = _count.ToString();


				if (_sleep < 250) _sleep = 250;
				CtrlZ();
				if (_count >= _MAX)
					break;

				Application.DoEvents();
				Thread.Sleep(_sleep);
			}
		}

		private void CtrlZ()
		{
			int keyCtrl = 17;
			int keyZ = 90;
			const int KEY_DOWN = 0x0000;
			const int KEY_UP = 0x0002;
			int kkk = 0;
			keybd_event((byte)keyCtrl,	0, KEY_DOWN, ref kkk);
			keybd_event((byte)keyZ,		0, KEY_DOWN, ref kkk);
			keybd_event((byte)keyZ,		0, KEY_UP, ref kkk);
			keybd_event((byte)keyCtrl,	0, KEY_UP, ref kkk);
		}

	}
}
