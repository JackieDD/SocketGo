using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketGo
{
    public partial class Client : Form
    {
        private Socket MySocket;
        public Client()
        {
            InitializeComponent();
            MySocket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_connect_Click(object sender, EventArgs e)
        {

            try
            {
                EndPoint point = new IPEndPoint(IPAddress.Parse(txt_IP.Text), int.Parse(txt_Port.Text));
                AppendMessage("开始连接...", false);
                MySocket.Connect(point); //连接
                AppendMessage("连接成功");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            } 
                
            

        }

        private void AppendMessage(string msg, bool newLine = true)
        {
            if (lbl_Msg.InvokeRequired)
            {
                lbl_Msg.Invoke(new ShowMessage(AppendMessage), new object[] { msg, newLine });
            }
            else
            {
                lbl_Msg.Text += (newLine ? Environment.NewLine : string.Empty) + msg;
            }

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.UTF8.GetBytes(txt_send.Text);
            MySocket.Send(data);
            AppendMessage("我："+txt_send.Text);
            txt_send.Text = string.Empty;
        }


        /// <summary>
        /// 屏蔽输入框enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      

        private void Txt_send_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 监听全局回车为发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_KeyPress(object sender, KeyPressEventArgs e)

        {
            if (e.KeyChar == '\r')
            {
                Button2_Click(button2, EventArgs.Empty);
            }
        }

      
    }
}
