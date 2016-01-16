using UnityEngine;
using System.Collections;

public class Projectilelanucher : MonoBehaviour
{


    public Rigidbody bullet;
    public Transform bulletPos;
    public Transform shooterMob;
    public float shotForce = 1000f;
    public float moveSpeed = 10f;
    public float fireRate = 3f;
    public float nextFire = 0.0f;


    void Update()
    {
        if (Time.time > nextFire)
        {

            nextFire = Time.time + fireRate;
            Rigidbody shot = Instantiate(bullet, bulletPos.position, bulletPos.rotation) as Rigidbody;
            shot.AddForce(bulletPos.forward * shotForce);

        }
    }

}
