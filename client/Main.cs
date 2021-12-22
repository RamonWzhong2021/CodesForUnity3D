using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	//人物模型预设

	public GameObject humanPrefab;
	public BaseHuman myHuman;
	public Dictionary<string, BaseHuman> otherHumans = new Dictionary<string, BaseHuman>();
	void Start()
	{
        NetManager.AddMsgListener(MsgType.MsgEnter, OnEnter);
        NetManager.AddMsgListener(MsgType.MsgListRes, OnListRes);
        NetManager.AddMsgListener(MsgType.MsgMove, OnMove);
        NetManager.AddMsgListener(MsgType.MsgLeave, OnLeave);
        NetManager.AddMsgListener(MsgType.MsgAttack, OnAttack);
        NetManager.AddMsgListener(MsgType.MsgDie, OnDie);

        NetManager.AddMsgListener(MsgType.MsgHit, OnMsgHit);
        NetManager.AddMsgListener(MsgType.MsgSyncPos, OnMsgSyncPos);
        NetManager.AddMsgListener(MsgType.MsgFire, OnMsgFire);



        NetManager.Connect("10.1.69.86", 8888);

		#region 添加一个角色
		GameObject obj = (GameObject)Instantiate(humanPrefab);
		float x = 500+ Random.Range(-50, 50);
		float z = 500 + Random.Range(-50, 50);
		obj.transform.position = new Vector3(x, 0, z);
		myHuman = obj.AddComponent<CtrlHuman>();
		//myHuman.desc = NetManager.GetDesc();
		#endregion

		#region 发送协议
        Vector3 pos = myHuman.transform.position;
        Vector3 eul = myHuman.transform.eulerAngles;
		MsgEnter msg = new MsgEnter(NetManager.GetDesc(), pos.x, pos.y,pos.z, eul.y);
		NetManager.Send(msg);
		
		MsgList msgList = new MsgList();
		NetManager.Send(msgList);
		#endregion
	}

	void Update()
	{
		NetManager.MsgUpdate();
	}

	void OnEnter(MsgBase msgBase)
	{
		MsgEnter msg = (MsgEnter)msgBase;
		Debug.Log("OnEnter "+ msg.desc);

		//是自己
		if (msg.desc == NetManager.GetDesc())
            return;

        // //添加一个角色
        GameObject obj = (GameObject)Instantiate(humanPrefab);
        obj.transform.position = new Vector3(msg.x, msg.y, msg.z);
        obj.transform.eulerAngles = new Vector3(0, msg.eulY, 0);
        BaseHuman h = obj.AddComponent<SyncHuman>();
        h.desc = msg.desc;
        otherHumans.Add(msg.desc, h);
    }

	void OnListRes(MsgBase msgBase)
	{
		MsgListRes msg = (MsgListRes)msgBase;
		Debug.Log("OnListRes ");
        foreach (Player player in msg.players)
        {
			if (player.desc == NetManager.GetDesc())
				continue;

            GameObject obj = (GameObject)Instantiate(humanPrefab);
            obj.transform.position = new Vector3(player.x, player.y, player.z);
            obj.transform.eulerAngles = new Vector3(0, player.eulY, 0);
            BaseHuman h = obj.AddComponent<SyncHuman>();
            h.desc = player.desc;
            otherHumans.Add(player.desc, h);
        }
		//解析参数
		// string[] split = msgArgs.Split(',');
		// int count = (split.Length - 1) / 6;
		// for (int i = 0; i < count; i++)
		{
			//string desc = split[i * 6 + 0];
			//是自己
			// if (desc == NetManager.GetDesc())
			// 	continue;
			// float x = float.Parse(split[i * 6 + 1]);
			// float y = float.Parse(split[i * 6 + 2]);
			// float z = float.Parse(split[i * 6 + 3]);
			// float eulY = float.Parse(split[i * 6 + 4]);
			// int hp = int.Parse(split[i * 6 + 5]);
			// //添加一个角色
			// GameObject obj = (GameObject)Instantiate(humanPrefab);
			// obj.transform.position = new Vector3(x, y, z);
			// obj.transform.eulerAngles = new Vector3(0, eulY, 0);
			// BaseHuman h = obj.AddComponent<SyncHuman>();
			// h.desc = desc;
			// otherHumans.Add(desc, h);
		}
	}

	void OnMove(MsgBase msgBase)
	{
		MsgMove msg = (MsgMove)msgBase;
		Debug.Log("OnMove " + msg.desc);
		//移动
		if (!otherHumans.ContainsKey(msg.desc))
			return;
		BaseHuman h = otherHumans[msg.desc];
		Vector3 targetPos = new Vector3(msg.x, msg.y, msg.z);
		h.MoveTo(targetPos);
	}

	void OnLeave(MsgBase msgBase)
	{
		MsgLeave msg = (MsgLeave)msgBase;
		Debug.Log("OnLeave " + msg.desc);
		//删除
		if (!otherHumans.ContainsKey(msg.desc))
			return;
		BaseHuman h = otherHumans[msg.desc];
		Destroy(h.gameObject);
		otherHumans.Remove(msg.desc);
	}

	void OnAttack(MsgBase msgBase)
	{
		MsgAttack msg = (MsgAttack)msgBase;
		Debug.Log("OnAttack " + msg.desc);
		//攻击动作
		if (!otherHumans.ContainsKey(msg.desc))
			return;
		SyncHuman h = (SyncHuman)otherHumans[msg.desc];
		h.SyncAttack(msg.eulY);
	}

	void OnDie(MsgBase msgBase)
	{
		MsgDie msg = (MsgDie)msgBase;
		Debug.Log("OnDie " + msg.desc);
		//自己死了
		if (msg.desc == myHuman.desc)
		{
			Debug.Log("Game Over");
			return;
		}
		//死了
		if (!otherHumans.ContainsKey(msg.desc))
			return;

		SyncHuman h = (SyncHuman)otherHumans[msg.desc];
		h.gameObject.SetActive(false);
		otherHumans.Remove(h.desc);
	}

	private void OnDestroy() {
		Debug.Log("OnDestroy");	
		NetManager.Close();
	}
}
