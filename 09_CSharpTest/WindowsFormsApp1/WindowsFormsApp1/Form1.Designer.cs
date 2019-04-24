namespace WindowsFormsApp1
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
			this.components = new System.ComponentModel.Container();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.Title = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(79, 160);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(107, 36);
			this.button1.TabIndex = 0;
			this.button1.Text = "start";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(95, 46);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(252, 21);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "새 탭";
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(219, 160);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(107, 36);
			this.button2.TabIndex = 2;
			this.button2.Text = "stop";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(95, 84);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(252, 21);
			this.textBox2.TabIndex = 3;
			this.textBox2.Text = "116";
			this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(154, 118);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(120, 21);
			this.numericUpDown1.TabIndex = 4;
			this.numericUpDown1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// Title
			// 
			this.Title.AutoSize = true;
			this.Title.Location = new System.Drawing.Point(32, 49);
			this.Title.Name = "Title";
			this.Title.Size = new System.Drawing.Size(29, 12);
			this.Title.TabIndex = 5;
			this.Title.Text = "Title";
			this.Title.Click += new System.EventHandler(this.label1_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(34, 87);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 12);
			this.label1.TabIndex = 6;
			this.label1.Text = "Key";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(34, 120);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 12);
			this.label2.TabIndex = 7;
			this.label2.Text = "Interval";
			// 
			// Form1
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(389, 230);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Title);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label Title;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}

