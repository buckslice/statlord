using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stats {
    public static string attack = "attack";
    public static string health = "health";
    public static string moveSpeed = "moveSpeed";
    public static string jumpSpeed = "jumpSpeed";
    public static string attackRate = "attackRate";
    public static string shotSpeed = "shotSpeed";
    public static string multishot = "multishot";
    // mitigation
    // pierce amount = 1.0f

}

// class instead of struct so it is passed by reference rather than value
public class Stat {

    public Stat(string name, float value, float increment) {
        this.name = name;
        this.value = value;
        this.increment = increment;
    }

    public string name;
    public float value;
    public float increment;
}

public class PlayerStats : MonoBehaviour {

    //array of stats
    private Stat[] stats = new Stat[] {
        new Stat(Stats.attack, 1.0f, 1.0f ),
        new Stat(Stats.health, 3.0f, 1.0f),
        new Stat(Stats.moveSpeed, 5.0f, 1.0f),

        new Stat(Stats.jumpSpeed, 5.0f, 1.0f),
        new Stat(Stats.attackRate, 1.0f, -.1f),
        new Stat(Stats.shotSpeed, 1000.0f, 100.0f),
        new Stat(Stats.multishot, 1.0f, 1.0f),

    };

    //private StatTable stats;
    private Dictionary<string, int> lookup = new Dictionary<string, int>();
    private ProjectileManager projectileManager;

    void Awake() {
        // build lookup table from stats
        for (int i = 0; i < stats.Length; i++) {
            lookup.Add(stats[i].name, i);
        }
        projectileManager = GameObject.Find("ProjectileManager").GetComponent<ProjectileManager>();
    }

    // return current stat
    public Stat get(string name) {
        int index;
        if (lookup.TryGetValue(name, out index)) {
            return stats[index];
        }
        return null;
    }

    // return current stat
    public Stat get(int index) {
        if (index < 0 || index >= stats.Length) {
            return null;
        }
        return stats[index];
    }

    public void fireProjectile(PType type) {
        // calculate damage dealt
        float damage = get(Stats.attack).value;

        Projectile p;
        p = projectileManager.getProjectile(type);
        p.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
        p.transform.rotation = transform.rotation;

        p.damage = damage;

        p.rb.AddForce(transform.forward * get(Stats.shotSpeed).value);

        p.gameObject.tag = Tags.PlayerProjectile;


        //int multishot = Mathf.RoundToInt(get(Stats.multishot).value);
        //for (int i = 0; i < multishot; ++i) {
        //    // probably change angle for each bullet depending on multishot
        //    // build and fire bullet        

        //}
    }


    public void changeHealth(float value) {
        get(Stats.health).value += value;
    }

}
