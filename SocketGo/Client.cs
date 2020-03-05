using System;
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
            byte[] data = Encoding.UTF8.GetBytes(txt_send.Text);
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
    }
}