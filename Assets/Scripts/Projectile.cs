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
        manager.returnProjectile(this);
    }
    void OnTriggerEnter(Collider c) {
        if(gameObject.tag == Tags.PlayerProjectile && c.gameObject.tag == Tags.Enemy) {
            manager.returnProjectile(this);
        }
    }

    void Update() {
        life -= Time.deltaTime;
        if (life <= 0.0f) {
            manager.returnProjectile(this);
        }
    }

}
