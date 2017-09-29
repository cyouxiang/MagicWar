using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerID : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        GetNetIdentity();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void GetNetIdentity () {
        this.gameObject.name = "Player_" + GetComponent<NetworkIdentity>().netId.ToString();
        if (isLocalPlayer) {
            GameObject player = GameObject.Find("Player_ID");
            player.GetComponent<Text>().text = ID();
        }
    }

    public string ID () {
        return this.gameObject.name;
    }
}
