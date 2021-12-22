using System;

[Serializable]
public class Player
{
    public string desc = "";
    public float x = 0, y = 0, z = 0;
    public float eulY = 0;
    public int hp = -100;
    //房间信息
    public int roomId = -1;
    public int seatId = -1;
}