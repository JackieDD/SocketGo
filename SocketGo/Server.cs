using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SocketGo
{
    public delegate void ShowMessage(string msg, bool newLine);

    public partial class Server : Form
    {
        private int m_numConnections;   // the maximum number of connections the sample is designed to handle simultaneously
        private int m_receiveBufferSize;// buffer size to use for each socket I/O operation
        private BufferManager m_bufferManager;  // represents a large reusable set of buffers for all socket operations
        private const int opsToPreAlloc = 2;    // read, write (don't alloc buffer space for accepts)
        private Socket listenSocket;            // the socket used to listen for incoming connection requests

        // pool of reusable SocketAsyncEventArgs objects for write, read and accept socket operations
        //SocketAsyncEventArgsPool m_readWritePool;
        private int m_totalBytesRead;           // counter of the total # bytes received by the server

        private int m_numConnectedSockets;      // the total number of clients connected to the server
        private Semaphore m_maxNumberAcceptedClients;

        private Socket clientSocket;
        private BufferManager bufferManager;
        private SocketAsyncEventArgs listenSocketAsyncEventArgs;
        private SocketAsyncEventArgs sendSocketAsyncEventArgs;
        private SocketAsyncEventArgs receiveSocketAsyncEventArgs;

        public Server()
        {
            InitializeComponent();
           var a= JsonConvert.SerializeObject(new byte[] { 1, 2, 3, 4, 5, 6 }.ToList());
            m_bufferManager = new BufferManager(1024 * 1 * opsToPreAlloc,
            1024);
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_ClickAsync(object sender, EventArgs e)
        {
            try
            {                


                button1.Enabled = false;
                IPAddress iPAddress = IPAddress.Parse(txt_IP.Text); //IP地址
                IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, int.Parse(txt_Port.Text)); //IP+端口号

                listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                listenSocket.Bind(iPEndPoint); //绑定
                listenSocket.Listen(int.MaxValue);
                AppendMessage("服务器开始监听...", false);

                listenSocketAsyncEventArgs = new SocketAsyncEventArgs();
                listenSocketAsyncEventArgs.Completed += SocketAsyncEventArgs_Completed;
                listenSocket.AcceptAsync(listenSocketAsyncEventArgs);

                //Thread thread = new Thread(AcceptMessage);
                // thread.Start(socket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                button1.Enabled = true;
            }
        }

        private void SocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            AppendMessage("客户端连接...");
            clientSocket = e.AcceptSocket;
            //var AcceptSocket = e.AcceptSocket;
            AppendMessage("客户地址: " + e.AcceptSocket.RemoteEndPoint);

            receiveSocketAsyncEventArgs = new SocketAsyncEventArgs();
            receiveSocketAsyncEventArgs.Completed += ReceiveSocketAsyncEventArgs_Completed;
            byte[] bytes = new byte[1024];
            receiveSocketAsyncEventArgs.SetBuffer(bytes, 0, 1024);
            clientSocket.ReceiveAsync(receiveSocketAsyncEventArgs);
        }

        private void ReceiveSocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                string message = Encoding.UTF8.GetString(e.Buffer);
                AppendMessage(listenSocketAsyncEventArgs.AcceptSocket.RemoteEndPoint + ":" + message);
                byte[] bytes = new byte[1024];
                receiveSocketAsyncEventArgs.SetBuffer(bytes, 0, 1024);
                clientSocket.ReceiveAsync(receiveSocketAsyncEventArgs);
            }
            else
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
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
                lbl_Msg.Invoke(new ShowMessage(AppendMessage), new object[] { msg, newLine });
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
            if (e.KeyChar == '\r')
            {
                Button2_Click(button2, EventArgs.Empty);
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
                e.Handled = true;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.UTF8.GetBytes(textBox1.Text);
            try
            {
                clientSocket.Send(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                try
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                }
                catch 
                {

                }
                clientSocket.Close();
            }
            
            AppendMessage("服务器：" + textBox1.Text);
            textBox1.Text = string.Empty;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (clientSocket.Connected)
            {
                try
                {
                    clientSocket?.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                }

                clientSocket?.Close();
            }
        }
    }
}