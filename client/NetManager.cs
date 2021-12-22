// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Net.Sockets;
// using UnityEngine.UI;
// using System;

// public static class NetManager
// {
// 	//定义套接字
// 	static Socket socket;
// 	//接收缓冲区
// 	static byte[] readBuff = new byte[1024];
// 	static int buffCount = 0;
// 	//委托类型
// 	public delegate void MsgListener(String str);
// 	//监听列表
// 	private static Dictionary<string, MsgListener> listeners = new Dictionary<string, MsgListener>();
// 	//消息列表
// 	static List<String> msgList = new List<string>();

// 	//添加监听
// 	public static void AddListener(string msgName, MsgListener listener)
// 	{
// 		listeners[msgName] = listener;
// 	}

// 	//获取描述
// 	public static string GetDesc()
// 	{
// 		if (socket == null) return "";
// 		if (!socket.Connected) return "";
// 		return socket.LocalEndPoint.ToString();
// 	}

// 	//连接
// 	public static void Connect(string ip, int port)
// 	{
// 		//Socket
// 		socket = new Socket(AddressFamily.InterNetwork,
// 			SocketType.Stream, ProtocolType.Tcp);
// 		socket.SendBufferSize=8192;
// 		socket.ReceiveBufferSize=8192;
// 		socket.NoDelay = true;
// 		socket.Ttl = 32;
// 		socket.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReuseAddress, true);
// 		socket.LingerState = new LingerOption (true, 10);

// 		socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive);

// 		//Connect
// 		socket.Connect(ip, port);
// 		//BeginReceive

// 		socket.BeginReceive(readBuff, 0, 1024, 0,
// 			ReceiveCallback, socket);
// 	}

	// //Receive回调
	// private static void ReceiveCallback(IAsyncResult ar)
	// {
	// 	try
	// 	{
	// 		Socket socket = (Socket)ar.AsyncState;
	// 		int count = socket.EndReceive(ar);
	// 		buffCount += count;

	// 		if(buffCount >= 2)
    //         {
	// 			//Int16 bodyLength = BitConverter.ToInt16(readBuff, 0);
	// 			Int16 bodyLength = (short)((readBuff[0] << 8) | readBuff[1]);

	// 			if (buffCount >= 2 + bodyLength)
    //             {
	// 				string recvStr = System.Text.Encoding.UTF8.GetString(readBuff, 2, bodyLength);
	// 				msgList.Add(recvStr);

	// 				Array.Copy(readBuff, 2 + bodyLength, readBuff, 0, buffCount - 2 - bodyLength);
	// 				buffCount = buffCount - 2 - bodyLength;
	// 			}
	// 		}

	// 		socket.BeginReceive(readBuff, buffCount, 1024-buffCount, 0,
	// 			ReceiveCallback, socket);
	// 	}
	// 	catch(ArgumentNullException ex)
	// 	{
	// 		Debug.Log("Socket Receive fail" + ex.ToString());
	// 	}
	// 	catch(ArgumentException ex){
	// 		Debug.Log("Socket Receive fail" + ex.ToString());
	// 	}	
	// 	catch (SocketException ex)
	// 	{
	// 		Debug.Log("Socket Receive fail" + ex.ToString());
	// 	}
	// }

// 	static Queue<ByteArray> writeQueue = new Queue<ByteArray>();
// 	//点击发送按钮
// 	public static void Send(string sendStr)
// 	{
// 		if (socket == null) return;
// 		if (!socket.Connected) return;

// 		byte[] bodyBytes = System.Text.Encoding.UTF8.GetBytes(sendStr);
// 		Int16 len = (Int16)bodyBytes.Length;
// 		byte[] lenBytes = BitConverter.GetBytes(len);
// 		if(BitConverter.IsLittleEndian)
//         {
// 			byte t = lenBytes[0];
// 			lenBytes[0] = lenBytes[1];
// 			lenBytes[1] = t;			
// 		}
// 		//byte[] sendBytes = lenBytes.Concat(bodyBytes);
// 		Array.Resize(ref lenBytes, 2 + len);
// 		byte[] sendBytes = lenBytes;
// 		Array.Copy(bodyBytes, 0, sendBytes, 2, len);
// 		//socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
// 		ByteArray ba = new ByteArray(sendBytes);
// 		int count = 0;
// 		lock (writeQueue)
// 		{
// 			writeQueue.Enqueue(ba);
// 			count = writeQueue.Count;
// 		}
// 		//send
// 		if (count == 1)
// 		{
// 			socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
// 		}
// 	}

// 	//Send回调
// 	private static void SendCallback(IAsyncResult ar)
// 	{
// 		try
// 		{
// 			Socket socket = (Socket)ar.AsyncState;
// 			int count = socket.EndSend(ar);
// 			ByteArray ba;
// 			lock (writeQueue)
// 			{
// 				ba = writeQueue.Peek();
// 			}
// 			ba.readIdx += count;
// 			if (ba.length == 0)
// 			{
// 				lock (writeQueue)
// 				{
// 					writeQueue.Dequeue();
// 					ba = writeQueue.Peek();
// 				}
// 			}
// 			if (ba != null)
// 			{
// 				socket.BeginSend(ba.bytes, ba.readIdx, ba.length, 0, SendCallback, socket);
// 			}
// 		}
// 		catch (SocketException ex)
// 		{
// 			Debug.Log("Socket Send fail" + ex.ToString());
// 		}
// 	}

// 	//Update
// 	public static void Update()
// 	{
// 		if (msgList.Count <= 0)
// 			return;
// 		String msgStr = msgList[0];
// 		msgList.RemoveAt(0);
// 		string[] split = msgStr.Split('|');
// 		string msgName = split[0];
// 		string msgArgs = split[1];
// 		//监听回调;
// 		if (listeners.ContainsKey(msgName))
// 		{
// 			listeners[msgName](msgArgs);
// 		}
// 	}
// }