using System;
using System.Net;
using System.Net.Sockets;
using ChatServer;


public class ClientState
{
    public Socket socket;
    public ByteArray recvBuff = new ByteArray();
    public ByteArray sendBuff=new ByteArray();
    //ping¼ä¸ô
    public static long pingInterval = 30;
    //Ping
    public long lastPingTime = 0;
    public Player player = new Player();

    public bool Expired(long curTime)
    {
        return (curTime - lastPingTime) > 4 * pingInterval;
    }
    public int roomId
    {
        get { return player.roomId; }
        set { player.roomId = value; }
    }
    public int seatId {
        get { return player.seatId; }
        set {
            player.seatId = value;
        }
    }
    public int hp {
        get { return player.hp; }
        set { player.hp = value; }
    }
    public string desc
    {
        get { return player.desc; }
        set { player.desc = value; }
    }

}