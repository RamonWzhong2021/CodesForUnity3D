using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Linq;


//事件
public enum NetEvent
{
    ConnectSucc = 1,
    ConnectFail = 2,
    Close = 3,
}
public static class NetManager {
    //定义套接字
    static Socket socket;
    //接收缓冲区
    static ByteArray readBuff;
    //写入队列
    static Queue<ByteArray> writeQueue;

    #region PING_PONG
    //是否启用心跳
    public static bool isUsePing = true;
    //心跳间隔时间
    public static int pingInterval = 30;
    //上一次发送PING的时间
    static float lastPingTime = 0;
    //上一次收到PONG的时间
    static float lastPongTime = 0;
    //发送PING协议

    private static void PingUpdate()
    {
        //是否启用
        if (!isUsePing)
        {
            return;
        }
        //发送PING
        if (Time.time - lastPingTime > pingInterval)
        {
            MsgPing msgPing = new MsgPing();
            Send(msgPing);
            lastPingTime = Time.time;
        }
        //检测PONG时间
        if (Time.time - lastPongTime > pingInterval * 4)
        {
            Close();
        }
    }
    //监听PONG协议
    private static void OnMsgPong(MsgBase msgBase)
    {
        Debug.Log("Pong");
        lastPongTime = Time.time;
    }
    #endregion

    #region EVENT_HANDLE
    //事件委托类型
    public delegate void EventListener(string err);
    //事件监听列表
    private static Dictionary<NetEvent, EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();
    //添加事件监听

    public static void AddEventListener(NetEvent netEvent, EventListener listener){
        //添加事件
        if (eventListeners.ContainsKey(netEvent)){
            eventListeners[netEvent] += listener;
        }
        //新增事件
        else{
            eventListeners[netEvent] = listener;
        }
    }

    //删除事件监听
    public static void RemoveEventListener(NetEvent netEvent, EventListener listener){
        if (eventListeners.ContainsKey(netEvent)){
            eventListeners[netEvent] -= listener;
        }
    }
    //分发事件
    private static void FireEvent(NetEvent netEvent, string err){
        if(eventListeners.ContainsKey(netEvent)){
            eventListeners[netEvent](err);
        }
    }
    #endregion

    #region CONNECT_SERVER
    static bool isConnecting = false;

    public static  string GetDesc()
    {
        while(!socket.Connected)
        {
            Thread.Sleep(10);
        }
        return socket.LocalEndPoint.ToString();
    }
    //连接
    public static void Connect(string ip, int port)
    {
        //状态判断
        if (socket != null && socket.Connected)
        {
            Debug.Log("Connect fail, already connected!");
            return;
        }
        if (isConnecting)
        {
            Debug.Log("Connect fail, isConnecting");
            return;
        }
        //初始化成员
        InitState();
        //Connect
        isConnecting = true;
        socket.BeginConnect(ip, port, ConnectCallback, socket);
    }
    //初始化状态
    private static void InitState()
    {
        //Socket
        socket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        //接收缓冲区
        readBuff = new ByteArray();
        //写入队列
        writeQueue = new Queue<ByteArray>();
        //参数设置
        socket.NoDelay = true;
        //是否正在连接
        isConnecting = false;
        //是否正在关闭
        isClosing = false;

        //消息列表
        msgList = new List<MsgBase>();
        //消息列表长度
        //msgCount = 0;

        lastPingTime = Time.time;
        lastPongTime = Time.time;
        if(!msgListeners.ContainsKey(MsgType.MsgPong))
        {
            AddMsgListener(MsgType.MsgPong, OnMsgPong);
        }
    }

    //Connect回调

    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Connect Succ ");
            FireEvent(NetEvent.ConnectSucc, "");
            isConnecting = false;
            //开始接收
            socket.BeginReceive(readBuff.bytes, readBuff.writeIdx,
                                            readBuff.remain, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Connect fail " + ex.ToString());
            FireEvent(NetEvent.ConnectFail, ex.ToString());
            isConnecting = false;
        }
    }
    #endregion

    #region SOCKET_CLOSE
    //是否正在关闭
    static bool isClosing = false;

    //关闭连接
    public static void Close()
    {
        //状态判断
        if (socket == null || !socket.Connected)
        {
            return;
        }
        if (isConnecting)
        {
            return;
        }

        //还有数据在发送
        if (writeQueue.Count > 0)
        {
            isClosing = true;
        }
        //没有数据在发送
        else
        {
            socket.Close();
            FireEvent(NetEvent.Close, "");
        }
    }
    #endregion

    //Receive回调

    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;

            int buffCount = socket.EndReceive(ar);
            if (buffCount == 0)
            {
                Close();
                return;
            }
            readBuff.writeIdx += buffCount;

            //Debug.Log("buffCount: " + buffCount);
            while (readBuff.length > 4)
            {
                int msgTotalLen = 0;
                MsgBase msgBase = MsgBase.FromBytes(readBuff.bytes, readBuff.readIdx, readBuff.length, out msgTotalLen);
                if (msgBase == null)
                {
                    break;
                }
                lock (msgList)
                {
                    msgList.Add(msgBase);
                    //msgCount++;
                }
                readBuff.readIdx += msgTotalLen;
            }

            if (readBuff.readIdx > 0)
                readBuff.MoveBytes();

            socket.BeginReceive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0, ReceiveCallback, socket);

        }

        catch (SocketException ex)

        {

            Debug.Log("Socket Receive fail" + ex.ToString());

        }
    }

    #region SEND_HANDLER

    //点击发送按钮
    public static void Send(MsgBase msgBase)

    {

        Send(MsgBase.ToBytes(msgBase));

    }

    //点击发送按钮
    public static void Send(byte[] sendBytes)
    {
        if (socket == null || !socket.Connected) return;
        if (isClosing) return;

        ByteArray ba = new ByteArray(sendBytes);
        lock (writeQueue)
        {
            writeQueue.Enqueue(ba);
            if (writeQueue.Count == 1)
            {
                socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
            }
        }
    }
    //Send回调
    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            if (socket == null || !socket.Connected)
                return;

            int count = socket.EndSend(ar);
            ByteArray ba;

            lock (writeQueue)
            {
                ba = writeQueue.First();
            }

            ba.readIdx += count;
            if (ba.length == 0)
            {
                lock (writeQueue)
                {
                    writeQueue.Dequeue();
                    ba = (writeQueue.Count <= 0) ? null : writeQueue.First();
                }
            }

            if (ba != null)
            {
                socket.BeginSend(ba.bytes, ba.readIdx, ba.length, 0, SendCallback, socket);
            }
            else if (isClosing)
            {
                socket.Close();
                socket = null;
                isClosing = false;
            }
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Send fail" + ex.ToString());
        }
    }
    #endregion

    #region MSG_LISTENER
    //消息委托类型
    public delegate void MsgListener(MsgBase msgBase);
    //消息监听列表
    private static Dictionary<MsgType, MsgListener> msgListeners = new Dictionary<MsgType, MsgListener>();

    //添加监听
    public static void AddMsgListener(MsgType type, MsgListener listener)
    {
        //添加
        if (msgListeners.ContainsKey(type))
        {
            msgListeners[type] += listener;
        }
        //新增
        else
        {
            msgListeners[type] = listener;
        }

    }

    //删除消息监听
    public static void RemoveMsgListener(MsgType type, MsgListener listener)
    {
        if (msgListeners.ContainsKey(type))
        {
            msgListeners[type] -= listener;
        }
    }

    //分发消息
    private static void FireMsg(MsgType type, MsgBase msgBase)
    {
        if (msgListeners.ContainsKey(type))
        {
            msgListeners[type](msgBase);
        }
    }
    #endregion

    #region RECEIVE_HANDLER

    //消息列表
    static List<MsgBase> msgList = new List<MsgBase>();
    //消息列表长度
    //static int msgCount = 0;
    //每一次Update处理的消息量
    readonly static int MAX_MESSAGE_FIRE = 10;
    #endregion

    //更新消息

    public static void MsgUpdate()
    {
        PingUpdate();
        //初步判断，提升效率
        //if (msgCount == 0)
        //{
        //    return;
        //}

        //重复处理消息
        for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
        {
            //获取第一条消息
            MsgBase msgBase = null;
            lock (msgList)
            {
                if (msgList.Count > 0)
                {
                    msgBase = msgList[0];
                    msgList.RemoveAt(0);
                    //msgCount--;
                }
            }

            //分发消息
            if (msgBase != null)
            {
                FireMsg(msgBase.msgType, msgBase);
            }
            //没有消息了
            else
            {
                break;
            }
        }

    }
}