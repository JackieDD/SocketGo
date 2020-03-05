namespace SocketGo
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_connect = new System.Windows.Forms.Button();
            this.txt_Port = new System.Windows.Forms.TextBox();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.txt_send = new System.Windows.Forms.TextBox();
            this.lbl_Msg = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "端口号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "IP";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(685, 411);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "发送消息";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(685, 16);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 23);
            this.btn_connect.TabIndex = 14;
            this.btn_connect.Text = "连接";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.Btn_connect_Click);
            // 
            // txt_Port
            // 
            this.txt_Port.Location = new System.Drawing.Point(285, 18);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(100, 21);
            this.txt_Port.TabIndex = 13;
            this.txt_Port.Text = "50000";
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(85, 18);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(100, 21);
            this.txt_IP.TabIndex = 12;
            this.txt_IP.Text = "127.0.0.1";
            // 
            // txt_send
            // 
            this.txt_send.Location = new System.Drawing.Point(40, 311);
            this.txt_send.Multiline = true;
            this.txt_send.Name = "txt_send";
            this.txt_send.Size = new System.Drawing.Size(720, 69);
            this.txt_send.TabIndex = 19;
            this.txt_send.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_send_KeyPress);
            // 
            // lbl_Msg
            // 
            this.lbl_Msg.Enabled = false;
            this.lbl_Msg.Location = new System.Drawing.Point(40, 57);
            this.lbl_Msg.Multiline = true;
            this.lbl_Msg.Name = "lbl_Msg";
            this.lbl_Msg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lbl_Msg.Size = new System.Drawing.Size(720, 212);
            this.lbl_Msg.TabIndex = 20;
            this.lbl_Msg.TextChanged += new System.EventHandler(this.Lbl_Msg_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(585, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "断开";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl_Msg);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.txt_Port);
            this.Controls.Add(this.txt_IP);
            this.KeyPreview = true;
            this.Name = "Client";
            this.Text = "Client";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Client_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.TextBox txt_Port;
        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.TextBox txt_send;
        private System.Windows.Forms.TextBox lbl_Msg;
        private System.Windows.Forms.Button button1;
    }
}