using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading;

public class NetManager
{
    private string listenIP = "0.0.0.0";
    private Int16 listenPort = 0;
    //监听Socket
    private  Socket listenfd;
    //客户端Socket及状态信息
    public Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();
    //Select的检查列表
     List<Socket> checkRead = new List<Socket>();
    private  bool bIsRunning = false;
    private RoomManager roomManager = new RoomManager();

    public static  NetManager Instance = new NetManager();

    public void StartLoop(string ip, Int16 port)
    {
        Instance.bIsRunning = !Instance.bIsRunning;
        if (Instance.bIsRunning)
        {
            Instance.listenPort = port;
            Instance.listenIP = ip;
            Thread t = new Thread(Instance.MainLoop);
            t.Start(Instance);
        }
    }

    public bool isRunning
    {
        get { return Instance.bIsRunning; }
    }

    public void StopLoog()
    {
        Instance.bIsRunning = false;
        Logger.Log("[服务器]停止服务！！");
    }

    private void MainLoop(object data)
    {
        //Socket
        listenfd = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        //Bind
        IPAddress ipAdr = IPAddress.Parse(listenIP);
        IPEndPoint ipEp = new IPEndPoint(ipAdr, listenPort);
        listenfd.Bind(ipEp);
        //Listen
        listenfd.Listen(0);
        Logger.Log("[服务器]启动成功...");

        //循环
        while (bIsRunning)
        {
            ResetCheckRead();  //重置checkRead
            Socket.Select(checkRead, null, null, 10000);
            //检查可读对象
            for (int i = checkRead.Count - 1; i >= 0; i--)
            {
                Socket s = checkRead[i];
                if (s == listenfd)
                {
                    AcceptClient(s);
                }
                else
                {
                    ReadClientfd(s);
                }
            }
            //超时
            Timer();
        }

        foreach (ClientState item in clients.Values)
        {
            Close(item);
        }
        listenfd.Close();
        listenfd = null;
    }

    //填充checkRead列表
    public  void ResetCheckRead()
    {
        checkRead.Clear();
        checkRead.Add(listenfd);
        foreach (ClientState s in clients.Values)
        {
            checkRead.Add(s.socket);
        }
    }
    public void AddUser(Socket client, ClientState state)
    {
        roomManager.Add(state);
        clients.Add(client, state);
    }

    public void RemoveUser(ClientState state)
    {
        roomManager.Remove(state);
        clients.Remove(state.socket);
    }

    //读取Listenfd
    public void AcceptClient(Socket listenfd)
    {
        try
        {
            Socket clientfd = listenfd.Accept();
            Logger.Log("Accept " + clientfd.RemoteEndPoint.ToString());
            ClientState state = new ClientState();
            state.socket = clientfd;
            state.lastPingTime =EventHandler.GetTimeStamp();
            state.desc = clientfd.RemoteEndPoint.ToString();
            AddUser(clientfd, state);
        }
        catch (SocketException ex)
        {
            Logger.Log("Accept fail" + ex.ToString());
        }
    }

    //读取Clientfd
    public  void ReadClientfd(Socket clientfd)
    {
        ClientState state = clients[clientfd];
        ByteArray recvBuff = state.recvBuff;

        //接收
        int count = 0;
        //缓冲区不够，清除，若依旧不够，只能返回
        //缓冲区长度只有1024，单条协议超过缓冲区长度时会发生错误，根据需要调整长度
        if (recvBuff.remain <= 0)
        {
            OnReceiveData(state);
            recvBuff.MoveBytes();
        };

        if (recvBuff.remain <= 0)
        {
            Logger.Log("Receive fail , maybe msg length > buff capacity");
            Close(state);
            return;
        }

        try
        {
            count = clientfd.Receive(recvBuff.bytes, recvBuff.writeIdx, recvBuff.remain, 0);
        }
        catch (SocketException ex)
        {
            Logger.Log("Receive SocketException " + ex.ToString());
            Close(state);
            return;
        }

        //客户端关闭
        if (count <= 0)
        {
            Logger.Log("Socket Close " + clientfd.RemoteEndPoint.ToString());
            Close(state);
            return;
        }

        //消息处理
        recvBuff.writeIdx += count;
        //处理二进制消息
        OnReceiveData(state);
        //移动缓冲区
        recvBuff.CheckAndMoveBytes();
    }

    //关闭连接

    public  void Close(ClientState state)
    {
        //消息分发
        MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
        object[] ob = { state };
        mei.Invoke(null, ob);
        //关闭
        state.socket.Close();
        RemoveUser(state);
    }

    //数据处理
    public  void OnReceiveData(ClientState state)
    {
        ByteArray recvBuff = state.recvBuff;
        while (recvBuff.length >=4)//处理粘包
        {
            int msgBytesLen= 0;
            //解析消息
            MsgBase msgBase = MsgBase.FromBytes(recvBuff.bytes, recvBuff.readIdx, recvBuff.length, out msgBytesLen);
            if (msgBase == null)
                break;  //半包数据
            recvBuff.readIdx += msgBytesLen;
            //分发消息
            MethodInfo mi = typeof(MsgHandler).GetMethod(msgBase.msgType.ToString());
            object[] o = { state, msgBase };
            Logger.Log("Receive " + msgBase.msgType);
            if (mi != null)
            {
                mi.Invoke(null, o);
            }
            else
            {
                Logger.Log("OnReceiveData Invoke fail " + msgBase.msgType);
            }
        }
        recvBuff.CheckAndMoveBytes();
    }
    //定时器
     void Timer()
    {
        //消息分发
        MethodInfo mei = typeof(EventHandler).GetMethod("OnTimer");
        object[] ob = { };
        mei.Invoke(null, ob);
    }
    public void Send(ClientState cs, byte[] bytes, int offset, int length)
    {
        //状态判断
        if (cs == null || cs.socket == null || !cs.socket.Connected)
        {
            return;
        }
        ByteArray ba = new ByteArray(bytes, offset, length);

        //为简化代码，暂不适用发送缓冲区
        try
        {
            cs.socket.BeginSend(ba.bytes, ba.readIdx, ba.length, 0, null, null);
        }
        catch (SocketException ex)
        {
            Logger.Log("Socket Close on BeginSend" + ex.ToString());
        }
    }

    //发送
    public void Send(ClientState cs, MsgBase msg)
    {
        //状态判断
        if (cs == null||cs.socket==null ||!cs.socket.Connected)
        {
            return;
        }
        byte[] bytes = MsgBase.ToBytes(msg);
        ByteArray ba = new ByteArray(bytes, 0, bytes.Length);

        //为简化代码，暂不使用发送缓冲区
        try
        {
            cs.socket.BeginSend(ba.bytes, ba.readIdx, ba.length, 0, null, null);
        }
        catch (SocketException ex)
        {
            Logger.Log("Socket Close on BeginSend" + ex.ToString());
        }
    }
}