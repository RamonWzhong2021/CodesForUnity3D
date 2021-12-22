using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MsgHandler
{
    public static void MsgRoom(ClientState cs, MsgBase msgBase)
    {
        MsgRoom msgRoom = (MsgRoom)msgBase;

        msgRoom.seatId = cs.seatId;
        msgRoom.roomId = cs.roomId;

        NetManager.Instance.Send(cs, msgRoom);
    }
    public static void MsgEnter(ClientState c, MsgBase msgBase)
    {
        MsgEnter msgMove = (MsgEnter)msgBase;
        byte[] bytes = MsgBase.ToBytes(msgMove);

        //广播
        foreach (ClientState cs in NetManager.Instance.clients.Values)
        {
            NetManager.Instance.Send(cs, bytes, 0, bytes.Length);
        }

    }

    public static void MsgList(ClientState c, MsgBase msgBase)
    {
        List<ClientState> list = new List<ClientState>(NetManager.Instance.clients.Values);
        MsgListRes msgList = new MsgListRes();
        foreach (ClientState cs in list)
        {
            msgList.players.Add(cs.player);
        }
        NetManager.Instance.Send(c, msgList);

    }
    public static void MsgMove(ClientState c, MsgBase msgBase)
    {
        byte[] bytes = MsgBase.ToBytes(msgBase);
        foreach (ClientState cs in NetManager.Instance.clients.Values)
        {
            NetManager.Instance.Send(cs, bytes, 0, bytes.Length);
        }
    }
    public static void MsgAttack(ClientState c, MsgBase msgBase)
    {
        MsgAttack msg = (MsgAttack)msgBase;

        byte[] bytes = MsgBase.ToBytes(msgBase);
        foreach (ClientState cs in NetManager.Instance.clients.Values)
        {
            NetManager.Instance.Send(cs, bytes, 0, bytes.Length);
        }
    }
    public static void MsgPing(ClientState c, MsgBase msgBase)
    {
        MsgPong msg = new MsgPong();
        byte[] bytes = MsgBase.ToBytes(msg);
        c.lastPingTime = EventHandler.GetTimeStamp();
        NetManager.Instance.Send(c, bytes, 0, bytes.Length);
    }
    public static void MsgHit(ClientState c, MsgBase msgBase)
    {
        MsgHit msgAttack = (MsgHit)msgBase;
        //被攻击
        ClientState hitCS = null;

        foreach (ClientState cs in NetManager.Instance.clients.Values)
        {
            if (cs.socket.RemoteEndPoint.ToString() == msgAttack.hitDesc)
            {
                hitCS = cs;
                break;
            }
        }

        if (hitCS == null)
            return;

        hitCS.hp -= 25;
        if (hitCS.hp <= 0)
        {
            MsgDie msgDie = new MsgDie(hitCS.socket.RemoteEndPoint.ToString());
            byte[] bytes = MsgBase.ToBytes(msgDie);
            foreach (ClientState cs in NetManager.Instance.clients.Values)
            {
                NetManager.Instance.Send(cs, bytes, 0, bytes.Length);
            }
        }

    }
}
