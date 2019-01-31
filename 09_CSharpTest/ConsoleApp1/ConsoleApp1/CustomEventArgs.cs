using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	class CustomEventArgs :EventArgs
	{
		private string msg;
		public string Message
		{
			get { return msg; }
		}
		public CustomEventArgs (string _s)
		{
			msg = _s;
		}
	}
}
