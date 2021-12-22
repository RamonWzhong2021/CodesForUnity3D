using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RoomManager
{
    private List<Room> roomList = new List<Room>();

    public int Count
    {
        get { return roomList.Count; }
    }

    public int Add(ClientState cs)
    {
        int id = 0;
        foreach (Room item in roomList)
        {
            id = item.Add(cs);
            if (id>0)
            {
                return id;
            }
        }
        Room room = new Room();
        id = room.Add(cs);
        roomList.Add(room);
        return id;
    }

    public void Remove(ClientState cs)
    {
        foreach (Room item in roomList)
        {
            if (item.Remove(cs))
                return;
        }
    }

}
