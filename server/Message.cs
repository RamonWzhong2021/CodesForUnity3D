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
    public MsgMove(string desc,float x, float y, float z, float eulY) {
        msgType = MsgType.MsgMove;
        this.desc = desc;
        this.x = x;
        this.y = y;
        this.z = z;
        this.eulY = eulY;
    }
    public string desc;
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public float eulY = 0;
}
public class MsgCtrlTurret : MsgBase
{
    public MsgCtrlTurret() { msgType = MsgType.MsgCtrlTurret; }
    public MsgCtrlTurret(string desc, float axis)
    {
        msgType = MsgType.MsgCtrlTurret;
        this.desc = desc;
        this.axis = axis;
    }
    public string desc;
    public float axis = 0;
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
    public MsgLeave(string desc) { msgType = MsgType.MsgLeave;
        this.desc = desc;
    }
    public string desc = "";
}


public class MsgRoom : MsgBase
{
    public MsgRoom() { msgType = MsgType.MsgRoom; }
    public MsgRoom(int roomId,int seatId)
    {
        msgType = MsgType.MsgRoom;
        this.roomId = roomId;
        this.seatId = seatId;
    }
    public int roomId = 0;
    public int seatId = 0;
}

public class MsgDie: MsgBase
{
    public MsgDie() { msgType = MsgType.MsgDie; }
    public MsgDie(string desc)
    {
        msgType = MsgType.MsgDie;
        this.desc = desc;
    }
    public string desc = "";
}
//同步坦克信息
public class MsgSyncPos:MsgBase {
	public MsgSyncPos() { msgType = MsgType.MsgSyncPos;}
    public MsgSyncPos(string id, float x, float y, float z, float eulX, float eulY,float eulZ, float turretY){
        this.id = id;
        this.x = x;
        this.y = y;
        this.z = z;
        this.ex = eulX;
        this.ey = eulY;
        this.ez=eulZ;
        this.turretY = turretY;
    }
	//服务端补充
	public string id = "";		//哪个坦克
	//位置、旋转、炮塔旋转
	public float x = 0f;		
	public float y = 0f;
	public float z = 0f;
	public float ex = 0f;		
	public float ey = 0f;
	public float ez = 0f;
	public float turretY = 0f;	
}

//开火
public class MsgFire:MsgBase {
	public MsgFire() {msgType = MsgType.MsgFire;}
    public MsgFire(string id, float x, float y, float z, float eulX, float eulY,float eulZ){
        this.id = id;
        this.x = x;
        this.y = y;
        this.z = z;
        this.ex = eulX;
        this.ey = eulY;
        this.ez = eulZ;
    }
    //炮弹初始位置、旋转
	public float x = 0f;		
	public float y = 0f;
	public float z = 0f;
	public float ex = 0f;
	public float ey = 0f;
	public float ez = 0f;
	//服务端补充
	public string id = "";		//哪个坦克
}

//击中
public class MsgHit:MsgBase {
	public MsgHit() {msgType = MsgType.MsgHit;}
	//击中谁
	public string targetId = "";
	//击中点	
	public float x = 0f;		
	public float y = 0f;
	public float z = 0f;
	//服务端补充
	public string id = "";		//哪个坦克
	public int hp = 0;			//被击中坦克血量
	public int damage = 0;		//受到的伤害
}
