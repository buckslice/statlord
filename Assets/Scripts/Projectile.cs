using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public Rigidbody rb;
    public float damage { get; set; }

    private ProjectileManager manager;
    private float life;

    // called my manager
    public void initialize(ProjectileManager manager) {
        rb = GetComponent<Rigidbody>();
        this.manager = manager;
        reset();
    }

    public void reset() {
        life = 3.0f;
        rb.velocity = Vector3.zero;
    }

    void OnCollisionEnter() {
        if (gameObject.name.Contains("Arrow"))
        {
            manager.returnArrow(this);
        }
        else 
        {
            manager.returnFireball(this);
        }
        //manager.returnProjectile(this);
    }
    void OnTriggerEnter(Collider c) {
        if(gameObject.tag == Tags.PlayerProjectile && c.gameObject.tag == Tags.Enemy) {
            if (gameObject.name.Contains("Arrow"))
            {
                manager.returnArrow(this);
            }
            else
            {
                manager.returnFireball(this);
            }
        }
    }

    void Update() {
        life -= Time.deltaTime;
        if (life <= 0.0f) {
            if (gameObject.name.Contains("Arrow"))
            {
                manager.returnArrow(this);
            }
            else
            {
                manager.returnFireball(this);
            }
            //manager.returnProjectile(this);
        }
    }

}
