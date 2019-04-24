namespace WindowsFormsApp2
{
	partial class Form1
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
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textInterval = new System.Windows.Forms.TextBox();
			this.textCount = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.display = new System.Windows.Forms.Label();
			this.textcontrol = new System.Windows.Forms.TextBox();
			this.textalpha = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(593, 49);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(78, 21);
			this.button1.TabIndex = 0;
			this.button1.Text = "지속매수";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(382, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "초 간격으로 ";
			// 
			// textInterval
			// 
			this.textInterval.Location = new System.Drawing.Point(310, 56);
			this.textInterval.Name = "textInterval";
			this.textInterval.Size = new System.Drawing.Size(66, 21);
			this.textInterval.TabIndex = 2;
			this.textInterval.Text = "0.3";
			// 
			// textCount
			// 
			this.textCount.Location = new System.Drawing.Point(461, 56);
			this.textCount.Name = "textCount";
			this.textCount.Size = new System.Drawing.Size(66, 21);
			this.textCount.TabIndex = 3;
			this.textCount.Text = "20";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(691, 49);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(78, 21);
			this.button2.TabIndex = 4;
			this.button2.Text = "멈춤";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// display
			// 
			this.display.AutoSize = true;
			this.display.Location = new System.Drawing.Point(470, 86);
			this.display.Name = "display";
			this.display.Size = new System.Drawing.Size(19, 12);
			this.display.TabIndex = 5;
			this.display.Text = "xx";
			// 
			// textcontrol
			// 
			this.textcontrol.Location = new System.Drawing.Point(46, 59);
			this.textcontrol.Name = "textcontrol";
			this.textcontrol.Size = new System.Drawing.Size(66, 21);
			this.textcontrol.TabIndex = 6;
			this.textcontrol.Text = "17";
			// 
			// textalpha
			// 
			this.textalpha.Location = new System.Drawing.Point(145, 59);
			this.textalpha.Name = "textalpha";
			this.textalpha.Size = new System.Drawing.Size(66, 21);
			this.textalpha.TabIndex = 7;
			this.textalpha.Text = "90";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(44, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(176, 12);
			this.label2.TabIndex = 8;
			this.label2.Text = "ctrl(17) a(65) s(83) z(90) x(88)";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textalpha);
			this.Controls.Add(this.textcontrol);
			this.Controls.Add(this.display);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.textCount);
			this.Controls.Add(this.textInterval);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textInterval;
		private System.Windows.Forms.TextBox textCount;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label display;
		private System.Windows.Forms.TextBox textcontrol;
		private System.Windows.Forms.TextBox textalpha;
		private System.Windows.Forms.Label label2;
	}
}

