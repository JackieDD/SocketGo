using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SocketGo
{
    public partial class Client : Form
    {
        private  Socket MySocket;
        private  SocketAsyncEventArgs connectSocketAsyncEventArgs;
        private SocketAsyncEventArgs receiveSocketAsyncEventArgs;
        private SocketAsyncEventArgs sendSocketAsyncEventArgs;

        public Client()
        {
            InitializeComponent();
            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
                btn_connect.Enabled = false;
                EndPoint point = new IPEndPoint(IPAddress.Parse(txt_IP.Text), int.Parse(txt_Port.Text));
                AppendMessage("开始连接...", false);

                connectSocketAsyncEventArgs = new SocketAsyncEventArgs
                {
                    RemoteEndPoint = point
                };
                connectSocketAsyncEventArgs.Completed += ConnectSocketAsyncEventArgs_Completed;

                //MySocket.Connect(point); //连接

                MySocket.ConnectAsync(connectSocketAsyncEventArgs);


                AppendMessage("连接成功");



                //连接成功 开始接收消息
                //Thread thread = new Thread(AcceptMessage);
                //thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                btn_connect.Enabled = true;
            }
        }

        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectSocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                MySocket = e.ConnectSocket;
                receiveSocketAsyncEventArgs = new SocketAsyncEventArgs();
                receiveSocketAsyncEventArgs.Completed += ReceiveSocketAsyncEventArgs_Completed;
                byte[] bytes = new byte[1024];
                receiveSocketAsyncEventArgs.SetBuffer(bytes, 0, 1024);
                MySocket.ReceiveAsync(receiveSocketAsyncEventArgs);

                sendSocketAsyncEventArgs = new SocketAsyncEventArgs();
                sendSocketAsyncEventArgs.Completed += SendSocketAsyncEventArgs_Completed;
            }
        }

        private void SendSocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            AppendMessage("我：" +  Encoding.UTF8.GetString( e.Buffer));

            txt_send.Invoke(new Action(() =>
            {
                txt_send.Text = string.Empty;
            }));
        }

        private void ReceiveSocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                string message = Encoding.UTF8.GetString(e.Buffer);
                AppendMessage(MySocket.RemoteEndPoint + ":" + message);
                byte[] bytes = new byte[1024];
                e.SetBuffer(bytes, 0, 1024);
                MySocket.ReceiveAsync(e);
            }
            else
            {
                if (MySocket.Connected)
                {
                    MySocket.Shutdown(SocketShutdown.Both);
                    MySocket.Close();
                }
               
            }

        }

        private void AcceptMessage()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs
                {
                };

                //socketAsyncEventArgs.SetBuffer(0, 1024);
                //接收到字节数组

                if (MySocket.Connected)
                {
 MySocket.Receive(bytes);
                //socketAsyncEventArgs.Completed += SocketAsyncEventArgs_Completed;
                //MySocket.ReceiveAsync(socketAsyncEventArgs);

                string message = Encoding.UTF8.GetString(bytes);
                AppendMessage(MySocket.RemoteEndPoint.ToString() + ":" + message);
                Console.WriteLine("msg"+message);
                }
                else
                {
                    MySocket?.Shutdown(SocketShutdown.Both);
                    MySocket?.Close();
                }
               
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
            string optionName = "Command";
            if (txt_send.Text.StartsWith("GUID"))
            {
                optionName = "GUID";
            }else if (txt_send.Text.StartsWith("Read"))
            {
                optionName = "Read";
                txt_send.Text = JsonConvert.SerializeObject(new List<byte>( new byte[]{ 19, 20, 29, 33, 38, 37, 37, 36, 36, 36, 36, 37, 37, 37, 37, 36, 36, 35, 34, 33, 32, 31, 30, 29, 28, 26, 24, 22, 20, 18, 16, 14, 8, 12, 21, 26, 32, 31, 31, 30, 30, 30, 31, 31, 32, 31, 31, 31, 31, 30, 30, 30, 30, 29, 29, 29, 29, 27, 25, 23, 21, 19, 17, 15, 2, 6, 15, 19, 24, 23, 23, 22, 22, 22, 23, 23, 23, 23, 22, 22, 22, 21, 20, 19, 19, 18, 17, 16, 15, 13, 12, 10, 8, 6, 4, 2, 109, 109, 114, 114, 119, 119, 117, 117, 117, 117, 117, 117, 117, 114, 114, 114, 114, 114, 111, 111, 111, 111, 111, 111, 111, 111, 111, 111, 111, 111, 111, 111,0xff,0x00,0x50,0x00,
                0,0x51,0x00,
                    0,0x52,0x00,
                0,0x53,0x00,
                0,0x54,0x00,
                0} ));

            }

            string jData = JsonConvert.SerializeObject(new RemoteFittingMessage
            {
                RL = 0,
                OptionName = optionName,
                Data = txt_send.Text,
                Index = 0
            });
            byte[] data = Encoding.UTF8.GetBytes(jData);

            sendSocketAsyncEventArgs.SetBuffer(data, 0, data.Length);
            MySocket.SendAsync(sendSocketAsyncEventArgs);
            //MySocket.Send(data);
            
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

        private void Button1_Click(object sender, EventArgs e)
        {

            MySocket?.Shutdown(SocketShutdown.Both);
            MySocket?.Close();
        }

        private void Lbl_Msg_TextChanged(object sender, EventArgs e)
        {
            lbl_Msg.SelectionStart = lbl_Msg.Text.Length;
            // lbl_Msg.SelectionLength = 0;
            lbl_Msg.ScrollToCaret();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            string b = "[-1,-2]";

           var bb =JsonConvert.DeserializeObject<int[]>(b).Select(s=>(byte)s);

        }
    }

    public class RemoteFittingMessage
    {
        public int RL { get; set; }
        public string OptionName { get; set; }

        public int Index { get; set; }
        public string Data { get; set; }
    }
}