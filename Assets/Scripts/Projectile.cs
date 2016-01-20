using UnityEngine;
using System.Collections;

public enum PType {
    ARROW,
    FIREBALL
};

public class Projectile : MonoBehaviour {

    public Rigidbody rb { get; set; }
    public float damage { get; set; }

    private ProjectileManager manager;
    private float life;
    public PType type;
    private bool dying = false;
    public bool freeze;
    public float pierce;
    private GameObject mesh;
    private Collider col;
    private ParticleSystem.EmissionModule psem;
    private Light lightComp;

    // called my manager
    public void initialize(ProjectileManager manager, PType type) {
        mesh = transform.Find("Model").gameObject;
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        psem = transform.Find("Particles").GetComponent<ParticleSystem>().emission;
        lightComp = GetComponent<Light>();
        this.manager = manager;
        this.type = type;
        freeze = false;
        reset();
    }

    public void reset() {
        mesh.SetActive(true);
        psem.enabled = true;
        col.enabled = true;
        if (lightComp) {
            lightComp.enabled = true;
        }
        dying = false;
        freeze = false;
        life = 4.0f;
        pierce = 0.0f;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider col) {
        if (gameObject.tag == Tags.PlayerProjectile && col.gameObject.tag == Tags.Enemy) {
            if (freeze) {
                StartCoroutine(Freeze(col));
            }
            manager.player.projectileHit(damage);
            if (type == PType.ARROW) {
                if (pierce >= 1.0f) {
                    pierce -= 1.0f;
                } else if (pierce > 0.0f) {
                    if (Random.value > pierce) {
                        returnSelf();
                    } else {
                        pierce -= 1.0f;
                    }
                } else {
                    returnSelf();
                }
            }
        }

        if (col.gameObject.tag == Tags.Wall) {
            returnSelf();
        }
    }

    void Update() {
        life -= Time.deltaTime;
        if (life <= 0.0f) {
            returnSelf();
        }
    }

    IEnumerator Freeze(Collider col) {

        Color orig = col.GetComponentInChildren<MeshRenderer>().material.color;
        col.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        col.GetComponent<EnemyBasicScript>().frozen = true;
        yield return new WaitForSeconds(3.0f);
        try {
            col.GetComponent<EnemyBasicScript>().frozen = false;
            col.GetComponentInChildren<MeshRenderer>().material.color = orig;
        } catch {

        }

    }

    private IEnumerator dieLater() {
        mesh.SetActive(false);
        psem.enabled = false;
        col.enabled = false;
        rb.isKinematic = true;
        if (lightComp) {
            lightComp.enabled = false;
        }

        yield return new WaitForSeconds(5.0f);

        manager.returnProjectile(this);

    }

    public void returnSelf() {
        if (dying) {
            return;
        }
        dying = true;

        StartCoroutine(dieLater());
    }
}
