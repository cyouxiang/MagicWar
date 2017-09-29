using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Damage : NetworkBehaviour {

    //private PlayerController playerController;
    private CameraController cameraController;
    //private CapsuleCollider capsuleCollider;
    //private Damage damage;
    public Animator playerAnim;
    public Animator monsterAnim;

    private Image HP_BAR;
    private GameObject DEAD_BG;
    int HP;
	private int HP_MAX = 3;
    public const int HPMON_MAX = 1;

    //int rotaSpeed;

    void Awake () {
        if (this.gameObject.name != "Monster") {
            GameObject HP_IMAGE = this.transform.Find("Canvas").Find("HP").gameObject;
            HP_BAR = HP_IMAGE.GetComponent<Image>();
            HP = HP_MAX;
        }

        if (this.gameObject.tag == "Monster") {
			HP = HPMON_MAX;
		}

        if (isLocalPlayer) {
        //    playerController = GetComponent<PlayerController>();
            cameraController = transform.Find("Main Camera").GetComponent<CameraController>();
        //    capsuleCollider = GetComponent<CapsuleCollider>();
        //    damage = GetComponent<Damage>();
        }
    }
	void Start () {
        
    }

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.name == "MagicBullet") {
			other.gameObject.GetComponent<MagicBulletController> ().DestroyMagicBullet();
			CmdHitPlayer(this.transform.gameObject);
        }

        if (other.gameObject.name == "Monster") {
            print("被咬了");
            CmdHitPlayer(this.transform.gameObject);
        }
		
	}
		
	[Command]
	void CmdHitPlayer(GameObject g) {
        g.GetComponent<Damage>().RpcResolveHit();
	}
		
	[ClientRpc]
	public void RpcResolveHit () {
		if (isLocalPlayer) {
			if (HP > 1) {
				HP--;
				HpBar ();
			} else {
                HP--;
                HpBar();
                print("DIE");

                if (!DEAD_BG) {
                    CmdDie();

                    DEAD_BG = Instantiate(Resources.Load("BG")) as GameObject;
                    DEAD_BG.name = "BG";
                    DEAD_BG.transform.parent = this.transform.Find("Canvas").transform;
                }
            }
		}

        if (this.gameObject.tag == "Monster") {
            if (HP > 1) {
                HP--;
            } else {
                HP--;
                print("MonsterDIE");
                CmdMonsterDie();
            }
        }
    }

    void Go () {
        if (isLocalPlayer) {
            CmdGo();

            HP = HP_MAX;

            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
            HpBar();

            CancelInvoke("Dead");
        }
    }
 
	void HpBar () {
        HP_BAR.fillAmount = (float)HP / (float)HP_MAX;
	}

    void Alive (bool b) {
        if (isLocalPlayer) {
            //    playerController.enabled = b;
            //    cameraController.enabled = b;
            //    capsuleCollider.enabled = b;
            //    damage.enabled = b;
        }
        //rotaSpeed = 270;

    }

    [Command]
    void CmdGo () {
        RpcGo();
    }

    [ClientRpc]
    void RpcGo () {
        if (isLocalPlayer) {
            Alive(true);
            //cameraController.enabled = true;
            Destroy(DEAD_BG);
        }
        playerAnim.SetTrigger("Go");
    }

    [Command]
    void CmdDie () {
        RpcDie();
    }

    [ClientRpc]
    void RpcDie () {
        //if (isLocalPlayer) {
            Alive(false);
            //cameraController.enabled = false;
            InvokeRepeating("Dead", 0.1f, 0.02f);
            Invoke("Go", 5.0f);

            playerAnim.SetTrigger("Die");
        //}
    }

    [Command]
    void CmdMonsterDie () {
        RpcMonsterDie();
    }

    [ClientRpc]
    void RpcMonsterDie () {
        if (this.gameObject.tag == "Monster") {
            Invoke("CmdGoDie", 5.0f);
            monsterAnim.SetTrigger("Die");
        }
    }

    [Command]
    void CmdGoDie () {
        RpcGoDie();
    }

    [ClientRpc]
    void RpcGoDie () {
        NetworkServer.Destroy(this.gameObject);
    }

    void Dead () {
        if (DEAD_BG) {
            DEAD_BG.GetComponent<Image>().color += new Color(0, 0, 0, 0.003f);
        }
        //transform.Rotate(Vector3.right * Time.deltaTime * -30);
		//transform.Rotate(Vector3.up * Time.deltaTime * rotaSpeed);
		//rotaSpeed -= 2;
	}
}
