using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour {

    public GameObject projectile;
    private List<Projectile> pool;

    void Awake() {
        pool = new List<Projectile>();

        for (int i = 0; i < 100; i++) {
            buildProjectile();
        }
    }

    public void buildProjectile() {
        GameObject x = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        x.transform.parent = transform;
        x.SetActive(false);
        Projectile p = x.GetComponent<Projectile>();
        p.initialize(this);
        pool.Add(p);
    }

    public Projectile getProjectile() {
        if (pool.Count <= 0) {
            buildProjectile();
        }
        int last = pool.Count - 1;
        Projectile p = pool[last];
        pool.RemoveAt(last);
        p.gameObject.SetActive(true);
        return p;
    }

    public void returnProjectile(Projectile p) {
        p.reset();
        p.gameObject.SetActive(false);
        pool.Add(p);
    }

}
