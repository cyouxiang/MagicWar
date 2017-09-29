using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MagicBulletController : NetworkBehaviour {

    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject[] trailParticles;
    //[HideInInspector]
    //public Vector3 impactNormal; //Used to rotate impactparticle.

    private float lifeTime;
    public float maxTime = 3.0f;

    // Use this for initialization
    void Start () {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;

        //NetworkServer.Spawn(projectileParticle);

        lifeTime = 0.0f;
    }

    [ServerCallback]
    void Update () {
        lifeTime += Time.deltaTime;
        if (lifeTime > maxTime) {
            //NetworkServer.Destroy(gameObject);
            DestroyMagicBullet();//改由方法銷毀
        }
    }

    public void DestroyMagicBullet() {
		NetworkServer.Destroy(gameObject);
	}

    void OnCollisionEnter (Collision hit) {

        impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.up)) as GameObject;
        NetworkServer.Spawn(impactParticle);
        
        Destroy(projectileParticle, 3f);
        Destroy(impactParticle, 5f);
        Destroy(gameObject);
    
    }
}
