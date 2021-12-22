using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

public enum MsgType : Int16
{
    MsgInvalid = 0,
    MsgEnter,
    MsgMove,
    MsgList,
    MsgListRes,
    MsgHit,
    MsgDie,
    MsgAttack,
    MsgPing,
    MsgPong,
    MsgLeave,
    MsgRoom,
    MsgPut,
    MsgCtrlTurret,
    MsgSyncPos,
    MsgFire,
    MsgMax,
}

public class MsgBase
{
    public Int16 msgLen; //正文的字节长度
    public MsgType msgType;

    //编码器
    static JavaScriptSerializer Js = new JavaScriptSerializer();
    //编码

    public static byte[] ToBytes(MsgBase msgBase)
    {
        string s = Js.Serialize(msgBase);
        byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(s);
        msgBase.msgLen = (Int16)strBytes.Length;

        byte[] buf = new byte[strBytes.Length + 4];
        WriteInt16(buf, 0, 2, msgBase.msgLen);
        WriteInt16(buf, 2, 2, (Int16)msgBase.msgType);
        strBytes.CopyTo(buf, 4);
        return buf;        
    }

    //解码
    public static MsgBase FromBytes(byte[] bytes, int offset, int count, out int msgBytesLen)
    {
        msgBytesLen = 0;
        if (count < 4) return null;
        Int16 msgLen = ReadInt16(bytes, offset, count);
        offset += 2;
        count -= 2;
        if (msgLen > count) return null;

        MsgType type = (MsgType)ReadInt16(bytes, offset, count);
        offset += 2;
        count -= 2;
        string s = System.Text.Encoding.UTF8.GetString(bytes, offset, msgLen);
        msgBytesLen = msgLen + 4;
        MsgBase msgBase = (MsgBase)Js.Deserialize(s, Type.GetType(type.ToString()));
        msgBase.msgLen = msgLen;
        return msgBase;
    }

    //编码协议名（2字节长度+字符串）
    private static void WriteInt16(byte[] buf, int offset, int count, Int16 val)
    {
        if (BitConverter.IsLittleEndian)
        {
            buf[offset] = (byte)(val & 0xff);
            buf[offset + 1] = (byte)(val >> 8);
        }
        else
        {
            buf[offset] = (byte)(val >> 8);
            buf[offset + 1] = (byte)(val & 0xff);
        }
    }
        //解码协议名（2字节长度+字符串）
    private static Int16 ReadInt16(byte[] bytes, int offset, int count)
    {
        //必须大于2字节
        if (count < 2)
        {
            throw new Exception("Need more data");
        }
        Int16 val;
        if (BitConverter.IsLittleEndian)
        {
            val = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);
        }
        else
        {
            val = (Int16)((bytes[offset] << 8) | bytes[offset + 1]);
        }

        return val;
    }
}