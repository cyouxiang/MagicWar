using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIReset : MonoBehaviour {
    RectTransform UISET;
    public GameObject playerName;
    public float offsetXY = 0f;
    GameObject playerCamera;
    Vector3 pos;
    Vector3 pos_scr;

    void Start () {
        //Reset();
    }
	
	// Update is called once per frame
	//void Update () {
    //    if (playerName) {
    //        //UIpos();
    //    } else {
    //        Destroy(this.gameObject);
    //    }
    //}

    void FixedUpdate () {
		if (playerName) {
			if(offsetXY == 0)UIpos ();
		} else
			Destroy (this.gameObject);
	}

    void Reset () {
        //UISET = this.transform.GetComponent<RectTransform>();
        //UISET.localScale = new Vector3(1, 1, 1);
        //UISET.offsetMin = new Vector2(0, 0);
        //UISET.offsetMax = new Vector2(0, 0);
        //transform.FindChild("ID").GetComponent<Text>().text = this.gameObject.name;
        //playerCamera = GameObject.FindWithTag("LocalCamera");

        UISET = this.transform.GetComponent<RectTransform> ();
		UISET.localScale = new Vector3 (1, 1, 1);
		UISET.offsetMin = new Vector2 (0 + offsetXY, 0 + offsetXY);
		UISET.offsetMax = new Vector2 (0 - offsetXY, 0 - offsetXY);
        print(offsetXY);
		if (offsetXY == 0) {
			transform.Find ("ID").GetComponent<Text> ().text = this.gameObject.name;
			playerCamera = GameObject.FindWithTag ("LocalCamera");
		} else {
			transform.Find ("Player_ID").GetComponent<Text> ().text = this.gameObject.name;
		}
    }

    void UIpos () {
        pos = new Vector3(playerName.transform.position.x, playerName.transform.position.y + playerName.GetComponent<Collider>().bounds.size.y * 0.75f, playerName.transform.position.z);
        pos_scr = playerCamera.GetComponent<Camera>().WorldToScreenPoint(pos);
        //Debug.Log (pos_scr);
        Debug.DrawLine(playerName.transform.position, pos_scr, Color.red);
        if (pos_scr.z > 0) {
            UISET.anchorMin = new Vector2(pos_scr.x / Screen.width - 0.05f, pos_scr.y / Screen.height - 0.01f);
            UISET.anchorMax = new Vector2(pos_scr.x / Screen.width + 0.05f, pos_scr.y / Screen.height);
        } else {
            UISET.anchorMin = new Vector2(1, 1);
        }
    }
}
