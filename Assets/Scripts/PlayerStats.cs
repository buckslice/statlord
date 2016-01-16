using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stats {
    public static string attack = "attack";
    public static string health = "health";
    public static string moveSpeed = "moveSpeed";
    public static string jumpSpeed = "jumpSpeed";
    public static string attackSpeed = "attackSpeed";
    public static string multishot = "multishot";

}

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
        new Stat(Stats.attackSpeed, 2.0f, -.1f),
        new Stat(Stats.multishot, 1.0f, 1.0f),

    };

    //private StatTable stats;
    private Dictionary<string, int> lookup = new Dictionary<string, int>();
    public int level = 1;   // determines how many stats are available


    void Awake() {
        // build lookup table from stats
        for (int i = 0; i < stats.Length; i++) {
            lookup.Add(stats[i].name, i);
        }
    }

    // return current stat
    public Stat get(string name) {
        int index = lookup[name];
        return stats[index];
    }

    public void fireBullet(GameObject bullet, float shotForce) {
        // calculate damage dealt
        float damage = get(Stats.attack).value;

        GameObject playerShot = Instantiate(bullet, transform.position + new Vector3(0, 2, 0) + transform.forward, Quaternion.identity) as GameObject;

        playerShot.GetComponent<Rigidbody>().AddForce(transform.forward * shotForce);


        int multishot = Mathf.RoundToInt(get(Stats.multishot).value);
        for (int i = 0; i < multishot; ++i) {
            // probably change angle for each bullet depending on multishot
            // build and fire bullet        
        }
    }

    public void changeHealth(float value) {
        get(Stats.health).value += value;
    }

    // Update is called once per frame
    void Update() {
        if (get(Stats.health).value <= 0.0f) {
            // die
        }

    }
}
