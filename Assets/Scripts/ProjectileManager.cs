using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour {

    public GameObject arrow,fireball;
    private List<Projectile> arrowPool, fireballPool;

    void Awake() {
        arrowPool = new List<Projectile>();
        fireballPool = new List<Projectile>();

        for (int i = 0; i < 100; i++) {
            buildArrow();
            buildFireball();
        }
    }

    public void buildArrow() {
        GameObject x = Instantiate(arrow, transform.position, Quaternion.identity) as GameObject;
        x.transform.parent = transform;
        x.SetActive(false);
        Projectile p = x.GetComponent<Projectile>();
        p.initialize(this);
        arrowPool.Add(p);
    }

    public void buildFireball()
    {
        GameObject x = Instantiate(fireball, transform.position, Quaternion.identity) as GameObject;
        x.transform.parent = transform;
        x.SetActive(false);
        Projectile p = x.GetComponent<Projectile>();
        p.initialize(this);
        fireballPool.Add(p);
    }

    public Projectile getArrow() {
        if (arrowPool.Count <= 0) {
            buildArrow();
        }
        int last = arrowPool.Count - 1;
        Projectile p = arrowPool[last];
        arrowPool.RemoveAt(last);
        p.gameObject.SetActive(true);
        return p;
    }

    public void returnArrow(Projectile p) {
        p.reset();
        p.gameObject.SetActive(false);
        arrowPool.Add(p);
    }

    public Projectile getFireball()
    {
        if (fireballPool.Count <= 0)
        {
            buildFireball();
        }
        int last = fireballPool.Count - 1;
        Projectile p = fireballPool[last];
        fireballPool.RemoveAt(last);
        p.gameObject.SetActive(true);
        return p;
    }

    public void returnFireball(Projectile p)
    {
        p.reset();
        p.gameObject.SetActive(false);
        fireballPool.Add(p);
    }

}
