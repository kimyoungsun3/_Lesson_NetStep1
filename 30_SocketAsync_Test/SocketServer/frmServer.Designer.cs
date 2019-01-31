namespace SocketServer
{
	partial class frmServer
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnStart = new System.Windows.Forms.Button();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.listLog = new System.Windows.Forms.ListBox();
			this.listUser = new System.Windows.Forms.ListBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(118, 347);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "서버 시작";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(12, 349);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(100, 21);
			this.txtPort.TabIndex = 1;
			this.txtPort.Text = "8000";
			// 
			// listLog
			// 
			this.listLog.FormattingEnabled = true;
			this.listLog.ItemHeight = 12;
			this.listLog.Location = new System.Drawing.Point(12, 12);
			this.listLog.Name = "listLog";
			this.listLog.Size = new System.Drawing.Size(302, 328);
			this.listLog.TabIndex = 2;
			this.listLog.SelectedIndexChanged += new System.EventHandler(this.listLog_SelectedIndexChanged);
			// 
			// listUser
			// 
			this.listUser.FormattingEnabled = true;
			this.listUser.ItemHeight = 12;
			this.listUser.Location = new System.Drawing.Point(320, 12);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(162, 328);
			this.listUser.TabIndex = 3;
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(200, 347);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 4;
			this.btnStop.Text = "서버 중지";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// frmServer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(497, 382);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.listLog);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.btnStart);
			this.Name = "frmServer";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.ListBox listLog;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.Button btnStop;
	}
}

