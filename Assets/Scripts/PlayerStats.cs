using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stats {
    public static string attack = "attack";
    public static string health = "health";
    public static string moveSpeed = "moveSpeed";
    public static string attackRate = "attackRate";

    public static string fireballChance = "fireballChance";
    public static string jumpSpeed = "jumpSpeed";
    public static string shotSpeed = "shotSpeed";
    public static string multishot = "multishot";

    public static string mitigation = "mitigation";
    public static string pierce = "piercing";
    public static string healthpersec = "hp/second";
    public static string critChance = "critChance";
    public static string critDamage = "critDamage";
    public static string lifesteal = "lifesteal";
    public static string dodge = "dodge";
    public static string freeze = "freeze";
    public static string bloom = "bloom";
    public static string cameraShake = "cameraShake";

}

// class instead of struct so it is passed by reference rather than value
public class Stat {

    public Stat(string name, float value, float increment, float cap) {
        this.name = name;
        this.value = value;
        this.increment = increment;
        this.cap = cap;
    }

    public float roundedValue() {
        return (float)System.Math.Round(value, 2);
    }

    public string name;
    public float value;
    public float increment;
    public float cap;
}

public class PlayerStats : MonoBehaviour {

    //array of stats
    private Stat[] stats = new Stat[] {
        //Core stats (Unlock in this order)
        new Stat(Stats.attack, 1.0f, 0.25f, 10.0f),               //implemented
        new Stat(Stats.health, 8.0f, 1.0f, 30.0f),              //implemented
        new Stat(Stats.moveSpeed, 5.0f, 0.5f, 10.0f),           //implemented

        //Additional stats (unlock at random)
        new Stat(Stats.attackRate, 1.0f, -.05f, 0.3f),          //implemented
        new Stat(Stats.fireballChance, 0.0f, 0.05f, 1.0f),      //implemented in Player.cs
        new Stat(Stats.jumpSpeed, 5.0f, 0.5f, 20.0f),           //implemented
        new Stat(Stats.shotSpeed, 8.0f, 1.0f, 20.0f),           //implemented
        new Stat(Stats.multishot, 1.0f, 0.25f, 3.0f),            //implemented in PlayerStats.cs as multishot
        new Stat(Stats.mitigation, 0.0f, 0.05f, 0.8f),          //implemented in Player.cs in OnTriggerEnter
        new Stat(Stats.pierce, 0.0f, 0.5f, 10.0f),              //implemented in Projectile.cs
        new Stat(Stats.healthpersec, 0.0f, 0.2f, 2.0f),          //implemented in Player.cs
        new Stat(Stats.critChance, 0.1f, 0.1f, 1.0f),          //implemented in PlayerStats.cs 
        new Stat(Stats.critDamage, 1.0f, 0.2f, 3.0f ),
        new Stat(Stats.lifesteal, 0.0f, 0.2f, 2.0f),            //implemented in EnemyBasicScript.cs
        new Stat(Stats.dodge, 0.0f, 0.05f, 1.0f),                //implemented in Player.cs in OnTriggerEnter
        new Stat(Stats.freeze, 0.0f, 0.1f, 1.0f),
        new Stat(Stats.bloom, 1.0f, 2.0f, 30.0f),              //implemented
        new Stat(Stats.cameraShake, 1.0f, 0.5f,5.0f),           //implemented
        
    };

    //private StatTable stats;
    private Dictionary<string, int> lookup = new Dictionary<string, int>();
    private ProjectileManager projectileManager;

    void Awake() {
        // build lookup table from stats
        Shuffle();
        for (int i = 0; i < stats.Length; i++) {
            lookup.Add(stats[i].name, i);
        }
        projectileManager = GameObject.Find("ProjectileManager").GetComponent<ProjectileManager>();

    }

    void Shuffle() {

        int n = stats.Length - 1;

        while (n > 3) {
            n--;
            int k = Random.Range(3, n);
            Stat x = stats[k];
            stats[k] = stats[n];
            stats[n] = x;
        }

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

    public int numStats() {
        return stats.Length;
    }

    public IEnumerator multiShot() {
        for (int i = 0; i < get(Stats.multishot).value; i++) {
            if (Random.value < get(Stats.fireballChance).value) {
                fireProjectile(PType.FIREBALL);
            } else {
                fireProjectile(PType.ARROW);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void fireProjectile(PType type) {
        // calculate damage dealt
        float damage = get(Stats.attack).value;

        Projectile p;
        p = projectileManager.getProjectile(type);
        p.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
        p.transform.rotation = transform.rotation;

        //crit chance
        p.damage = damage;
        if (Random.value < get(Stats.critChance).value) {
            p.damage += damage * get(Stats.critDamage).value;
        }
        p.pierce = get(Stats.pierce).value;
        if (get(Stats.freeze).value > Random.value) {
            p.freeze = true;
        }
        p.rb.AddForce(transform.forward * get(Stats.shotSpeed).value * 100.0f);

        p.gameObject.tag = Tags.PlayerProjectile;

    }

}
