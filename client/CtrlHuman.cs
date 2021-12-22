using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlHuman : BaseHuman
{

	// Use this for initialization
	new void Start()
	{
		base.Start();
		SkinnedMeshRenderer skinnedMeshRenderer = GameObject.Find("EthanBody").GetComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer.material.color = Color.red;
	}

	// Update is called once per frame
	new void Update()
	{
		base.Update();
		//移动
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Terrain")
			{
				MoveTo(hit.point);
				//发送协议
				string sendStr = "Move|";
				//sendStr += NetManager.GetDesc() + ",";
				sendStr += hit.point.x + ",";
				sendStr += hit.point.y + ",";
				sendStr += hit.point.z + ",";
				//NetManager.Send(sendStr);
				MsgMove move = new MsgMove(NetManager.GetDesc(), hit.point.x,hit.point.y,hit.point.z,0);
				NetManager.Send(move);
			}			
		}
		if (Input.GetMouseButtonDown(1))
		{
			if (isAttacking) return;
			if (isMoving) return;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)&& hit.collider.tag == "Terrain")
            {
				transform.LookAt(hit.point);
				Attack();

				MsgAttack attack = new MsgAttack();
				attack.desc = NetManager.GetDesc();
				attack.eulY = transform.eulerAngles.y;
				NetManager.Send(attack);

				#region 攻击判定
				Vector3 lineStart = transform.position + 0.5f * transform.up;
				Vector3 lineEnd = lineStart + 2 * transform.forward;

				if (Physics.Linecast(lineStart, lineEnd, out hit))
				{
					GameObject hitObj = hit.collider.gameObject;
					if (hitObj == gameObject)
						return;

					SyncHuman h = hitObj.GetComponent<SyncHuman>();
					if (h == null)
						return;

					MsgHit msgHit = new MsgHit();
					msgHit.attDesc = NetManager.GetDesc();
					msgHit.hitDesc = h.desc;
					NetManager.Send(msgHit);
				}

				#endregion
			}
		}		
	}
}