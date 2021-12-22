using System;
using System.Collections.Generic;
using System.Linq;

public class MsgEnter : MsgBase
{
    public MsgEnter() { msgType = MsgType.MsgEnter; }
    public MsgEnter(string desc, float x, float y, float z, float eulY)
    {
        msgType = MsgType.MsgEnter;
        this.desc = desc;
        this.x = x;
        this.y = y;
        this.z = z;
        this.eulY = eulY;
    }
    public string desc = "";
    public float x;
    public float y;
    public float z;
    public float eulY;
}

public class MsgMove : MsgBase
{
    public MsgMove() { msgType = MsgType.MsgMove; }
    public MsgMove(string desc, float x, float y, float z, int seatId)
    {
        msgType = MsgType.MsgMove;
        this.desc = desc;
        this.x = x;
        this.y = y;
        this.z = z;
        this.seatId = seatId;
    }
    public string desc;
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public int seatId = 0;
}


public class MsgAttack : MsgBase
{
    public MsgAttack() { msgType = MsgType.MsgAttack; }
    public string desc = "";
    public float eulY;
}

public class MsgHit : MsgBase
{
    public MsgHit() { msgType = MsgType.MsgHit; }
    public string attDesc = "";
    public string hitDesc = "";
}

public class MsgPing : MsgBase
{
    public MsgPing() { msgType = MsgType.MsgPing; }
}

public class MsgPong : MsgBase
{
    public MsgPong() { msgType = MsgType.MsgPong; }
}


public class MsgList : MsgBase
{
    public MsgList() { msgType = MsgType.MsgList; }
    // public MsgList(List<Player> players)
    // {
    //     msgType = MsgType.MsgList;
    //     this.players = players;  ///ugly !!
    // }
    // public List<Player> players = new List<Player>();
}
public class MsgListRes: MsgBase
{
    public MsgListRes() { msgType = MsgType.MsgListRes; }
    public MsgListRes(List<Player> players) { msgType = MsgType.MsgListRes;
        this.players = players;  ///ugly !!
    }
    public List<Player> players = new List<Player>();
}
public class MsgLeave : MsgBase
{
    MsgLeave() { msgType = MsgType.MsgLeave; }
    public MsgLeave(string desc)
    {
        msgType = MsgType.MsgLeave;
        this.desc = desc;
    }
    public string desc = "";
}


public class MsgRoom : MsgBase
{
    public MsgRoom() { msgType = MsgType.MsgRoom; }
    public MsgRoom(int roomId, int seatId)
    {
        msgType = MsgType.MsgRoom;
        this.roomId = roomId;
        this.seatId = seatId;
    }
    public int roomId = 0;
    public int seatId = 0;
}

public class MsgDie : MsgBase
{
    public MsgDie() { msgType = MsgType.MsgDie; }
    public MsgDie(string desc)
    {
        msgType = MsgType.MsgDie;
        this.desc = desc;
    }
    public string desc = "";
}


