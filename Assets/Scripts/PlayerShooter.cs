using UnityEngine;
using System.Collections;

public class PlayerShooter : MonoBehaviour {

    public Rigidbody bullet;
    public Transform playerBulletSpwnPos;
    public float shotForce = 1000f;
    public float fireRate = 4f;
    public float nextFire = 0.0f; 

	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Rigidbody playerShot = Instantiate(bullet, playerBulletSpwnPos.position, playerBulletSpwnPos.rotation) as Rigidbody;
            playerShot.AddForce(playerBulletSpwnPos.forward * shotForce);

        }
	
	}
}
