using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;

namespace ChatServer
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            Logger.Add(AppendText);
        }


        private void btnStartSvr_Click(object sender, EventArgs e)
        {
            if (!NetManager.Instance.isRunning)
            {
                NetManager.Instance.StartLoop(tbxSvrIP.Text, Int16.Parse(tbxPort.Text));
                btnStartSvr.Text = "停止服务器";
            }
            else
            {
                NetManager.Instance.StopLoog();
                btnStartSvr.Text = "启动服务器";
            }
        }

        //线程操作窗体中的rtbChatContent，需要用委托去实现
        delegate void AppendTextCallback(string text);
        private void AppendText(string text)
        {
            text += "\n";
            if (rtbChatContent.InvokeRequired)
            {
                AppendTextCallback callback = new AppendTextCallback(rtbChatContent.AppendText);
                this.Invoke(callback, new object[] { text });
            }
            else
                rtbChatContent.AppendText(text);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //主窗体退出了，因此线程也需要退出
            NetManager.Instance.StopLoog();
        }


        public static void Send(ClientState cs, string msg)
        {
            try
            {
                byte[] bodyBytes = System.Text.Encoding.UTF8.GetBytes(msg);
                Int16 len = (Int16)bodyBytes.Length;
                byte[] lenBytes = BitConverter.GetBytes(len);
                if (!BitConverter.IsLittleEndian)
                {
                    lenBytes.Reverse();
                }
                byte[] sendBytes = lenBytes.Concat<byte>(bodyBytes).ToArray();

                cs.sendBuff.Write(sendBytes, 0, sendBytes.Length);

                int ret = cs.socket.Send(cs.sendBuff.bytes, cs.sendBuff.readIdx, cs.sendBuff.length, 0);
                if (ret >= 0)
                {
                    cs.sendBuff.readIdx += ret;
                    cs.sendBuff.CheckAndMoveBytes();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
