using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EnemyHP : NetworkBehaviour {
    private GameObject HudBar;

    void Start () {
        //if (!isLocalPlayer)
        //    SpawnHpBar();

        //SpawnHpBar();
    }
	
	void Update () {
	
	}

    void SpawnHpBar () {
        //HudBar = Instantiate(Resources.Load("HP_Enemy")) as GameObject;
        //HudBar.name = "Player_" + GetComponent<NetworkIdentity>().netId.ToString();
        //
        //HudBar.transform.parent = GameObject.Find("Canvas").transform;
        //HudBar.GetComponent<UIReset>().playerName = this.gameObject;

        if (!isLocalPlayer) {
			HudBar = Instantiate (Resources.Load ("HP_Enemy")) as GameObject;
			HudBar.name = "Player_" + GetComponent<NetworkIdentity> ().netId.ToString ();
			HudBar.transform.parent = GameObject.Find ("Canvas").transform;
			HudBar.GetComponent<UIReset> ().playerName = this.gameObject;
		} else {
			HudBar = Instantiate (Resources.Load ("HP")) as GameObject;
			HudBar.name = "Player_" + GetComponent<NetworkIdentity> ().netId.ToString ();
			HudBar.transform.parent = GameObject.Find ("Canvas").transform;
			HudBar.GetComponent<UIReset> ().playerName = this.gameObject;
			HudBar.GetComponent<UIReset> ().offsetXY = 10f;
		}
		//this.gameObject.GetComponent<Damage> ().HP_BAR = HudBar.GetComponent<Image>();
    }
}
