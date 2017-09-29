using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SearchPlayer : NetworkBehaviour {

    UnityEngine.AI.NavMeshAgent Nav;
    public bool serverMonster;
    private Animator monsterAnim;
    private bool isDie = false;
    private AudioSource audioSource;

    void Start () {
	    if (isServer) {
			serverMonster = true;
			Nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();

            monsterAnim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        } else
			serverMonster = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!isDie) {
            Nav.SetDestination(GameObject.FindWithTag("Player").transform.position);
            
            monsterAnim.SetBool("Attack", true);
            
            if (CloseYou ()) {
				//target = null;
				//av.Stop(true);
                //onsterAnim.SetBool("Attack", false);
                //udioSource.Stop();
            }
        }
	}

    void OnTriggerStay(Collider other) {
		
		if(other.tag == "Player" && serverMonster){
			Nav.Resume ();
			//target = other.gameObject;
		}
	
	}

    void OnCollisionEnter(Collision other) {
		if (other.gameObject.name == "MagicBullet") {
            isDie = true;
            Nav.Stop();
            audioSource.Stop();
        }
	}

    bool CloseYou(){
		float dis = Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position);
        //print(dis);
		if (dis < 3f) {
			Nav.Stop ();
            //target.GetComponent<Damage> ().TakeDamage (3);
            monsterAnim.SetBool("Attack", false);
            audioSource.Stop();
            return true;
		} else {
			Nav.Resume ();
			return false;
		}
	}
}
