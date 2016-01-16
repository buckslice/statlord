using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stats {
    public static string attack = "attack";
    public static string health = "health";
    public static string moveSpeed = "moveSpeed";
    public static string multishot = "multishot";
    
}

public struct Stat {

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

    private List<Stat> stats = new List<Stat>() {
        new Stat(Stats.attack, 1.0f, 1.0f ),
        new Stat(Stats.health, 3.0f, 1.0f),
        new Stat(Stats.moveSpeed, 1.0f, .1f),


        new Stat(Stats.multishot, 1.0f, 1.0f),


    };

    private Dictionary<string, int> lookup = new Dictionary<string, int>();

    public int level = 1;

    // returns current value of stat
    public float getStatValue(string name) {
        int index = lookup[name];
        return stats[index].value;
    }

    void Awake() {
        // build lookup table from stats
        for(int i = 0; i < stats.Count; i++) {
            lookup.Add(stats[i].name, i);
        }
    }

    public void fireBullet() {
        // calculate damage dealt
        //float damage = 

        // build and fire bullet
    }

    public void takeDamage(float damage) {

    }

    // Update is called once per frame
    void Update() {

    }
}
