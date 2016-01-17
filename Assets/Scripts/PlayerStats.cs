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
    public static string pierce = "pierce";
    public static string healthRegen = "healthRegen";
    public static string critChance = "critChance";
    public static string roll = "roll";
    public static string healthOnKill = "healthOnKill";

    public static string randomize = "randomize";
    public static string enemySize = "enemySize";
    public static string cameraShake = "cameraShake";
    public static string playerSize = "playerSize";
    public static string deleteTextures = "deleteTextures";
    public static string increaseBloom = "increaseBloom";
    public static string doorsPerMinute = "doorsPerMinute";
    public static string uiSize = "uiSize";
    public static string misclick = "misclick";
}

// class instead of struct so it is passed by reference rather than value
public class Stat {

    public Stat(string name, float value, float increment, float cap) {
        this.name = name;
        this.value = value;
        this.increment = increment;
        this.cap = cap;
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
        new Stat(Stats.attack, 1.0f, 1.0f, 3.0f),               //implemented
        new Stat(Stats.health, 3.0f, 1.0f, 20.0f),              //implemented
        new Stat(Stats.moveSpeed, 5.0f, 0.5f, 10.0f),           //implemented
        new Stat(Stats.attackRate, 1.0f, -.05f, 0.1f),          //implemented

        //Additional stats (unlock at random)
        new Stat(Stats.fireballChance, 0.0f, 0.02f, 1.0f),      //implemented
        new Stat(Stats.jumpSpeed, 5.0f, 0.5f, 20.0f),           //implemented
        new Stat(Stats.shotSpeed, 8.0f, 1.0f, 20.0f),           //implemented
        new Stat(Stats.multishot, 1.0f, 0.25f, 5.0f),
        new Stat(Stats.mitigation, 0.0f, 0.05f, 0.8f),
        new Stat(Stats.pierce, 0.0f, 0.5f, 10.0f),
        new Stat(Stats.healthRegen, 0.0f, 0.2f, 2.0f),
        new Stat(Stats.critChance, 0.0f, 0.05f, 1.0f),
        new Stat(Stats.healthOnKill, 0.0f, 0.2f, 5.0f),

        //Joke stats (unlock at random)
        new Stat(Stats.increaseBloom, 1.25f, 2.0f, 30.0f),      //implemented
        //new Stat(Stats.randomize, 0.0f, 1.0f),
        //new Stat(Stats.enemySize, 1.0f, 0.1f),
        //new Stat(Stats.playerSize, 1.0f, 0.1f),
        //new Stat(Stats.cameraShake, 0.0f, 0.1f),
        //new Stat(Stats.deleteTextures, 0.0f, 2.5f),
        //new Stat(Stats.doorsPerMinute, 0.0f, 1.0f),
        //new Stat(Stats.uiSize, 1.0f, 0.1f),
        //new Stat(Stats.misclick, 0.0f, 2.5f)
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

    public int numStats() {
        return stats.Length;
    }

    public void fireProjectile(PType type) {
        // calculate damage dealt
        float damage = get(Stats.attack).value;

        Projectile p;
        p = projectileManager.getProjectile(type);
        p.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
        p.transform.rotation = transform.rotation;

        p.damage = damage;
        if (get(Stats.critChance).value < Random.Range(0, 100)) {
            p.damage += damage;
        }
        p.pierce = get(Stats.pierce).value;
        p.rb.AddForce(transform.forward * get(Stats.shotSpeed).value * 100.0f);

        p.gameObject.tag = Tags.PlayerProjectile;


        //int multishot = Mathf.RoundToInt(get(Stats.multishot).value);
        //for (int i = 0; i < multishot; ++i) {
        //    // probably change angle for each bullet depending on multishot
        //    // build and fire bullet        

        //}
    }

}
