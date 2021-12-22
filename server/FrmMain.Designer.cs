namespace ChatServer
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tbxSvrIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStartSvr = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rtbChatContent = new System.Windows.Forms.RichTextBox();
            this.tbxPort = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxSvrIP
            // 
            this.tbxSvrIP.Location = new System.Drawing.Point(137, 22);
            this.tbxSvrIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbxSvrIP.Name = "tbxSvrIP";
            this.tbxSvrIP.Size = new System.Drawing.Size(131, 25);
            this.tbxSvrIP.TabIndex = 0;
            this.tbxSvrIP.Text = "0.0.0.0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "服务器主机名：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(295, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "服务器端口：";
            // 
            // btnStartSvr
            // 
            this.btnStartSvr.Location = new System.Drawing.Point(503, 24);
            this.btnStartSvr.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStartSvr.Name = "btnStartSvr";
            this.btnStartSvr.Size = new System.Drawing.Size(91, 25);
            this.btnStartSvr.TabIndex = 3;
            this.btnStartSvr.Text = "启动服务器";
            this.btnStartSvr.UseVisualStyleBackColor = true;
            this.btnStartSvr.Click += new System.EventHandler(this.btnStartSvr_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("楷体", 10F);
            this.label4.Location = new System.Drawing.Point(15, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "消息显示框";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Location = new System.Drawing.Point(27, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 15);
            this.label5.TabIndex = 7;
            // 
            // rtbChatContent
            // 
            this.rtbChatContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbChatContent.Location = new System.Drawing.Point(0, 121);
            this.rtbChatContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rtbChatContent.Name = "rtbChatContent";
            this.rtbChatContent.ReadOnly = true;
            this.rtbChatContent.Size = new System.Drawing.Size(944, 615);
            this.rtbChatContent.TabIndex = 8;
            this.rtbChatContent.TabStop = false;
            this.rtbChatContent.Text = "";
            // 
            // tbxPort
            // 
            this.tbxPort.Location = new System.Drawing.Point(391, 21);
            this.tbxPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxPort.Mask = "99999";
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(92, 25);
            this.tbxPort.TabIndex = 2;
            this.tbxPort.Text = "8888";
            this.tbxPort.ValidatingType = typeof(int);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxPort);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbxSvrIP);
            this.groupBox1.Controls.Add(this.btnStartSvr);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(944, 72);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 72);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(944, 49);
            this.panel2.TabIndex = 9;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 736);
            this.Controls.Add(this.rtbChatContent);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "General Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxSvrIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnStartSvr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox rtbChatContent;
        private System.Windows.Forms.MaskedTextBox tbxPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
    }
}

