﻿using UnityEngine;
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
    private GameObject mesh;
    private Collider col;
    private ParticleSystem.EmissionModule psem;

    // called my manager
    public void initialize(ProjectileManager manager, PType type) {
        mesh = transform.Find("Model").gameObject;
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        psem = transform.Find("Particles").GetComponent<ParticleSystem>().emission;
        this.manager = manager;
        this.type = type;
        reset();
    }

    public void reset() {
        mesh.SetActive(true);
        psem.enabled = true;
        col.enabled = true;
        dying = false;
        life = 3.0f;
        rb.velocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider col) {
        if (type == PType.ARROW && gameObject.tag == Tags.PlayerProjectile && col.gameObject.tag == Tags.Enemy) {
            returnSelf();
        }

        if (col.gameObject.tag == Tags.Player)
        {
            returnSelf();
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

    private IEnumerator dieLater() {
        mesh.SetActive(false);
        psem.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(5.0f);

        manager.returnProjectile(this);

    }

    private void returnSelf() {
        if (dying) {
            return;
        }
        dying = true;

        StartCoroutine(dieLater());
    }
}
