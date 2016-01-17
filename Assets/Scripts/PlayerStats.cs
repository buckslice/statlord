using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stats {
    public static string attack = "attack";
    public static string health = "health";
    public static string moveSpeed = "moveSpeed";
    public static string jumpSpeed = "jumpSpeed";
    public static string attackSpeed = "attackSpeed";
    public static string attackForce = "attackForce";
    public static string multishot = "multishot";
    // mitigation
    // pierce amount = 1.0f

}

// these should be in player stats
//public float shotForce = 1000f;
//public float fireRate = 4f;
//public float nextFire = 0.0f; 

//public enum StatType {
//    attack,
//    health,
//    moveSpeed,
//    multishot
//};

//public class StatTable {
//    public Stat attack = new Stat(Stats.attack, 1.0f, 1.0f);
//    public Stat health = new Stat(Stats.health, 3.0f, 1.0f);
//    public Stat moveSpeed = new Stat(Stats.moveSpeed, 1.0f, 0.1f);
//    public Stat multishot = new Stat(Stats.multishot, 1.0f, 0.1f);
//}

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
        new Stat(Stats.attackSpeed, 1.0f, -.1f),
        new Stat(Stats.attackForce, 1000.0f, 100.0f),
        new Stat(Stats.multishot, 1.0f, 1.0f),

    };

    //private StatTable stats;
    private Dictionary<string, int> lookup = new Dictionary<string, int>();
    private ProjectileManager projectileManager;
    public int level = 1;
    // timers
    private float timeSinceAttack = 0.0f;

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

    public void fireProjectile() {
        // calculate damage dealt
        float damage = get(Stats.attack).value;

        Projectile p = projectileManager.getProjectile();
        p.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
        p.transform.rotation = transform.rotation;

        p.damage = damage;

        p.rb.AddForce(transform.forward * get(Stats.attackForce).value);

        p.gameObject.tag = "PlayerProjectile";


        //int multishot = Mathf.RoundToInt(get(Stats.multishot).value);
        //for (int i = 0; i < multishot; ++i) {
        //    // probably change angle for each bullet depending on multishot
        //    // build and fire bullet        

        //}
    }

    public void changeHealth(float value) {
        get(Stats.health).value += value;
    }

    // Update is called once per frame
    void Update() {
        if (get(Stats.health).value <= 0.0f) {
            // die
        }

        timeSinceAttack -= Time.deltaTime;
        if (Input.GetMouseButton(0) && timeSinceAttack < 0.0f) {
            timeSinceAttack = get(Stats.attackSpeed).value;
            fireProjectile();
        }

    }
}
