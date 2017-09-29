using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour {

    private PlayerController playerController;
    private CameraController cameraController;
    private CapsuleCollider capsuleCollider;
    //private Damage damage;
    private Camera playerCamera;
    private AudioListener playerAudioListener;
    private Canvas canvas;
    //private EventSystem eventSystem;

    public float speed = 5f;
    public float jumpSpeed = 400.0f;

    public Animator playerAnim;
    private AnimatorStateInfo currentState;

    private bool grounded = true; 
    private Vector3 direction;
    private Coroutine cououtine;

    public GameObject[] projectiles;
    public Transform attackPosition;
    public Transform cameraRotation;

    IEnumerator Move()
    {
        while (true)
        {
            transform.Translate(Vector3.forward * this.direction.y * Time.deltaTime * this.speed);
            transform.Translate(Vector3.right * this.direction.x * Time.deltaTime * this.speed);

            playerAnim.SetBool("Run", true);
            yield return null;
        }
    }

    void Start () {
        if (!isLocalPlayer) {
            playerController = GetComponent<PlayerController>();
            cameraController = transform.Find("Main Camera").GetComponent<CameraController>();
            //capsuleCollider = GetComponent<CapsuleCollider>();
            //damage = GetComponent<Damage>();
            playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
            playerAudioListener = transform.Find("Main Camera").GetComponent<AudioListener>();
            canvas = transform.Find("Canvas").GetComponent<Canvas>();
            //eventSystem = transform.FindChild("EventSystem").GetComponent<EventSystem>();

            playerController.enabled = false;
            cameraController.enabled = false;
            //capsuleCollider.enabled = false;
            //damage.enabled = false;
            playerCamera.enabled = false;
            playerAudioListener.enabled = false;
            canvas.enabled = false;
            //eventSystem.enabled = false;
        } else {
            //transform.FindChild("Main Camera").tag = "LocalCamera";
        }
    }

    void Update () {
        //print(grounded);
        if (isLocalPlayer) {
            //playerAnim.SetBool("Attack", false);

            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                if (hit.distance < 0.1f)
                    grounded = true;
            }

            //currentState = playerAnim.GetCurrentAnimatorStateInfo(0);
            //
            //if (currentState.nameHash == attackState) {
            //
            //}

        }
        //transform.Translate(Vector3.forward * CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime * this.speed);
        //transform.Translate(Vector3.right * CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * this.speed);

    }

    public void BeginMove () {
        //print(this.direction.y);
        StartCoroutine("Move");

        //transform.Translate(Vector3.forward * this.direction.y * Time.deltaTime * this.speed);
        //transform.Translate(Vector3.right * this.direction.x * Time.deltaTime * this.speed);
    }

    public void EndMove () {
        StopCoroutine("Move");

        playerAnim.SetBool("Run", false);
    }

    public void UpdateDirection (Vector3 direction) {
        //print(direction);
        this.direction = direction;
    }

    [Command]
    public void CmdJump() {
        //print(jumpSpeed);
        if (grounded == true) {
            print("跳");
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed);
            //this.GetComponent<Transform>().position = new Vector3(10, 10, 10);

            grounded = false;
        }
    }

    [Command]
    public void CmdAttack() {
        print("發射");
        
        //playerAnim.SetTrigger("Attack");
        RpcAttack();

        GameObject projectile = Instantiate(projectiles[0], attackPosition.position, cameraRotation.rotation) as GameObject;
        projectile.name = "MagicBullet";
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * 1000);
        //projectile.GetComponent<ProjectileScript>().impactNormal = hit.normal;

        NetworkServer.Spawn(projectile);
    }

    [ClientRpc]
    void RpcAttack () {
        playerAnim.SetTrigger("Attack");
    }
}