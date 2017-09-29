using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MonsterSpawner : NetworkBehaviour {

	public GameObject monster;
	public int monsterNum;
	public float R=3;
 
	void Start () {
		InvokeRepeating ("CmdScanMonster",3f ,10f);
	}
 
	[Command]
	void CmdSpawnMonster(){
		for(int i=0 ; i < monsterNum ; i++){
			Vector3 pos = new Vector3 (this.transform.position.x + Random.Range(-R,R) , this.transform.position.y , this.transform.position.z + Random.Range(-R,R) );
			GameObject mon = (GameObject)Instantiate (monster , pos ,this.transform.rotation);
			mon.name = "Monster";
			NetworkServer.Spawn (mon);
		}
	}
 
	[Command]
	void CmdScanMonster(){
		if (!GameObject.FindWithTag ("Monster")) {
			CmdSpawnMonster ();
		}
	}
}
