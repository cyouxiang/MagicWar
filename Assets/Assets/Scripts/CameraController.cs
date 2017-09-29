using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//增加觸碰操作功能，控制攝影機運動
public class CameraController : MonoBehaviour {

    public GameObject player;
    public float speed = 20f;

    private float fireTime = 0.0f;
    public float nextFireInterval = 1.5f;

    //紀錄手指觸碰位置
    Vector2 m_screenPos = new Vector2 ();

    void Start () {
        //允許多點觸碰
        Input.multiTouchEnabled = true;
    }

    void Update () {
        MobileInput ();
        //DeskopInput();
        //print(this.transform.eulerAngles.x);

        if (this.transform.eulerAngles.x < 315 && this.transform.eulerAngles.x > 270)
            this.transform.Rotate(new Vector3(1.0f, 0, 0));

        if (this.transform.eulerAngles.x > 45 && this.transform.eulerAngles.x < 90)
            this.transform.Rotate(new Vector3(-1.0f, 0, 0));

        fireTime += Time.deltaTime;
    }

    void DeskopInput () {
        
        //紀錄滑鼠左鍵的移動距離
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        if (mx != 0 || my != 0) {
            //滑鼠左鍵
            if (Input.GetMouseButton(0)) {
                //移動攝影機位置
                //this.transform.Translate(new Vector3(-mx * Time.deltaTime * speed, -my * Time.deltaTime * speed, 0));
                if (this.transform.eulerAngles.x < 315 && this.transform.eulerAngles.x > 45) {

                } else {
                    //移動攝影機
                    this.transform.Rotate(new Vector3(-my * Time.deltaTime * this.speed, 0, 0));
                }
                //移動角色
                player.GetComponent<Transform>().Rotate(new Vector3(0, mx * Time.deltaTime * this.speed, 0));

                if (fireTime > nextFireInterval) {
                    print("攻擊");
                    player.GetComponent<PlayerController>().CmdAttack();

                    fireTime = 0.0f;
                }
            }
        }
    }

    void MobileInput () {
        if (Input.touchCount <= 0)
            return;

        for (int i = 0; i < Input.touchCount; i++) {
            //判斷是否觸碰到IU介面
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId)) {
                //開始觸碰
                if (Input.GetTouch(i).phase == TouchPhase.Began) {
                    //紀錄觸碰位置
                    m_screenPos = Input.GetTouch(i).position;
                } else if (Input.GetTouch(i).tapCount > 2) {
                    if (fireTime > nextFireInterval) {
                        print("攻擊");
                        player.GetComponent<PlayerController>().CmdAttack();

                        fireTime = 0.0f;
                    }
                //手指移動
                } else if (Input.GetTouch(i).phase == TouchPhase.Moved) {
                    if (this.transform.eulerAngles.x < 315 && this.transform.eulerAngles.x > 45) {

                    } else {
                        //移動攝影機
                        this.transform.Rotate(new Vector3(-Input.GetTouch(i).deltaPosition.y * Time.deltaTime * this.speed, 0, 0));
                    }
                    //移動角色
                    player.GetComponent<Transform>().Rotate(new Vector3(0, Input.GetTouch(i).deltaPosition.x * Time.deltaTime * this.speed, 0));
                }
            }
        }
    }
}
