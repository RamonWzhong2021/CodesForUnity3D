using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public    class Room
{
    private static readonly int MAX_USER_NUM = 2;
    private List<ClientState> users = new List<ClientState>();

    public bool isFull
    {
        get { return users.Count >= MAX_USER_NUM; }
    }

    public int Add(ClientState cs)
    {
        if (!isFull)
        {
            users.Add(cs);
            cs.seatId = users.Count;
            cs.roomId = this.GetHashCode();
            return users.Count;
        }
        return -1;
    }
    public bool Remove(ClientState cs)
    {
        return users.Remove(cs);
    }

}
