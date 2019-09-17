using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketGo
{

    public delegate void ShowMessage(string msg, bool newLine);
    public partial class Server : Form
    {


        public Server()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 连接 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress iPAddress = IPAddress.Parse(txt_IP.Text); //IP地址
                IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, int.Parse(txt_Port.Text)); //IP+端口号
                                                                                             
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(iPEndPoint); //绑定
                socket.Listen(10);
                AppendMessage("服务器开始监听...", false);
                Thread thread = new Thread(AcceptMessage);
                thread.Start(socket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

        }

        Dictionary<string, Socket> dic = new Dictionary<string, Socket>();//通信用socket

        /// <summary>
        /// 接收socket信息
        /// </summary>
        private void AcceptMessage(object soc)
        {
            Socket socket = soc as Socket;
            while (true)
            {
                //创建通信用socket 阻塞方法
                Socket tSocket = socket.Accept();
                AppendMessage("客户端连接...");
                string point = tSocket.RemoteEndPoint.ToString();
                AppendMessage("客户端地址 "+point);
                Thread thread = new Thread(ReceiveMessage);
                thread.Start(tSocket);
            }
            
        }

        private void ReceiveMessage(object soc)
        {
            while (true)
            {
                Socket tSocket = soc as Socket;
                byte[] bytes = new byte[1024];
                //接收到字节数组
                tSocket.Receive(bytes);
                string message = Encoding.UTF8.GetString(bytes);
                AppendMessage(tSocket.RemoteEndPoint.ToString()+":"+message);
            }         
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="newLine"></param>
        private void AppendMessage(string msg, bool newLine = true)
        {
            if (lbl_Msg.InvokeRequired)
            {
                lbl_Msg.Invoke(new ShowMessage(AppendMessage),new object[] { msg,newLine});
            }
            else
            {
                lbl_Msg.Text += (newLine ? Environment.NewLine : string.Empty) + msg;
            }
           
        }

        /// <summary>
        /// 打开客户端界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Client_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        //滚动到最新
        private void Lbl_Msg_TextChanged(object sender, EventArgs e)
        {
            lbl_Msg.SelectionStart = lbl_Msg.Text.Length;
           // lbl_Msg.SelectionLength = 0;
            lbl_Msg.ScrollToCaret();
        }

        private void Server_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
