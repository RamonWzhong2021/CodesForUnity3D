using System;



class EventHandler
{

    public static void OnDisconnect(ClientState c)
    {
        string desc = c.socket.RemoteEndPoint.ToString();

        byte[] sendBytes = MsgBase.ToBytes(new MsgLeave(desc));

        foreach (ClientState cs in NetManager.Instance.clients.Values)
            NetManager.Instance.Send(cs, sendBytes, 0, sendBytes.Length);
    }
    public static void OnTimer()
    {
        CheckPing();
    }
    //获取时间戳
    public static long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

    //Ping检查
    private static void CheckPing()
    {
        //现在的时间戳
        long timeNow = GetTimeStamp();
        //遍历，删除
        foreach (ClientState s in NetManager.Instance.clients.Values)
        {
            if (s.Expired(timeNow))
            {
                Logger.Log("Ping Close " + s.socket.RemoteEndPoint.ToString());
                NetManager.Instance.Close(s);
                return;
            }
        }
    }
}
