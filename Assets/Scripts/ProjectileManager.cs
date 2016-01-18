using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ProjectileManager : MonoBehaviour {

    public GameObject arrow, fireball;
    private Dictionary<PType, List<Projectile>> poolLookup;

    void Awake() {
        // build pools for each type
        poolLookup = new Dictionary<PType, List<Projectile>>();
        PType[] ptypes = (PType[])Enum.GetValues(typeof(PType));
        for (int i = 0; i < ptypes.Length; ++i) {
            poolLookup[ptypes[i]] = new List<Projectile>();
        }

    }

    public void buildProjectile(PType type) {
        GameObject x;
        switch (type) {
            case PType.ARROW:
                x = Instantiate(arrow, transform.position, Quaternion.identity) as GameObject;
                break;
            case PType.FIREBALL:
                x = Instantiate(fireball, transform.position, Quaternion.identity) as GameObject;
                break;
            default:
                x = Instantiate(arrow, transform.position, Quaternion.identity) as GameObject;
                break;
        }

        x.transform.parent = transform;
        x.SetActive(false);
        Projectile p = x.GetComponent<Projectile>();
        p.initialize(this, type);

        poolLookup[type].Add(p);
    }

    public Projectile getProjectile(PType type) {
        List<Projectile> list = poolLookup[type];
        if (list.Count <= 0) {
            buildProjectile(type);
        }

        int last = list.Count - 1;
        Projectile p = list[last];
        list.RemoveAt(last);
        p.gameObject.SetActive(true);
        return p;
    }

    public void returnProjectile(Projectile p) {
        p.reset();
        p.gameObject.SetActive(false);
        poolLookup[p.type].Add(p);
    }

    public void destroyAll() {
        int childs = transform.childCount;
        for (int i = childs - 1; i > 0; i--) {
            returnProjectile(transform.GetChild(i).GetComponent<Projectile>());
        }
    }

}
