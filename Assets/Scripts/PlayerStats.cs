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
    public static string scattershot = "scattershot";
    public static string mitigation = "mitigation";
    public static string pierce = "pierce";
    public static string healthRegen = "healthRegen";
    public static string critChance = "critChance";
    public static string healthOnKill = "healthOnKill";
    public static string dodge = "dodge";

    public static string bloom = "bloom";
    public static string cameraShake = "cameraShake";
    //public static string randomize = "randomize";
    //public static string enemySize = "enemySize";
    //public static string playerSize = "playerSize";
    //public static string deleteTextures = "deleteTextures";
    //public static string doorsPerMinute = "doorsPerMinute";
    //public static string uiSize = "uiSize";
    //public static string misclick = "misclick";
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
        new Stat(Stats.fireballChance, 0.0f, 0.05f, 1.0f),      //implemented in Player.cs
        new Stat(Stats.jumpSpeed, 5.0f, 0.5f, 20.0f),           //implemented
        new Stat(Stats.shotSpeed, 8.0f, 1.0f, 20.0f),           //implemented
        new Stat(Stats.multishot, 1.0f, 0.25f, 3.0f),            //implemented in PlayerStats.cs as multishot
        new Stat(Stats.scattershot, 1.0f, 0.25f, 5.0f),
        new Stat(Stats.mitigation, 0.0f, 0.05f, 0.8f),          //implemented in Player.cs in OnTriggerEnter
        new Stat(Stats.pierce, 0.0f, 0.5f, 10.0f),              //implemented in Projectile.cs
        new Stat(Stats.healthRegen, 0.0f, 0.2f, 2.0f),          //implemented in Player.cs
        new Stat(Stats.critChance, 0.0f, 0.05f, 1.0f),          //implemented in PlayerStats.cs 
        new Stat(Stats.healthOnKill, 0.0f, 0.2f, 5.0f),         //implemented in EnemyBasicScript.cs
        new Stat(Stats.dodge, 0.0f, 1.0f, 0.5f),                //implemented in Player.cs in OnTriggerEnter



        //Joke stats (unlock at random)
        new Stat(Stats.bloom, 1.25f, 2.0f, 30.0f),              //implemented
        //new Stat(Stats.randomize, 0.0f, 1.0f),
        //new Stat(Stats.enemySize, 1.0f, 0.1f),
        //new Stat(Stats.playerSize, 1.0f, 0.1f),
        new Stat(Stats.cameraShake, 1.0f, 0.5f,5.0f),           //implemented
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

    public IEnumerator multiShot(PType type, int num)
    {
        for (int i =0; i<num;i++)
        {
            float damage = get(Stats.attack).value;

            Projectile p;
            p = projectileManager.getProjectile(type);
            p.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
            p.transform.rotation = transform.rotation;

            //crit chance
            p.damage = damage;
            if (Random.value < get(Stats.critChance).value)
            {
                p.damage += damage;
            }
            p.pierce = get(Stats.pierce).value;
            p.rb.AddForce(transform.forward * get(Stats.shotSpeed).value * 100.0f);

            p.gameObject.tag = Tags.PlayerProjectile;
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

    public IEnumerator multiScatterShot(PType type)
    {
        for (int i = 0; i < get(Stats.multishot).value;i++ )
        {

            scatterShot(type);
            yield return new WaitForSeconds(0.1f);
        }
            
    }

    public void scatterShot(PType type)
    {
        float damage = get(Stats.attack).value;

        if (get(Stats.scattershot).value==2.0)
        {
            Projectile proj = projectileManager.getProjectile(PType.ARROW);
            proj.gameObject.tag = Tags.EnemyProjectile;
            proj.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
            proj.transform.rotation = transform.rotation;
            proj.transform.position += transform.right * 0.7f;
                       

            Projectile proj2 = projectileManager.getProjectile(PType.ARROW);
            proj2.gameObject.tag = Tags.EnemyProjectile;
            proj2.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
            proj2.transform.rotation = transform.rotation;
            proj2.transform.position -= transform.right * 0.7f;

            proj.damage = damage;
            proj2.damage = damage;

            proj.rb.AddForce(proj.transform.forward * 800);
            proj2.rb.AddForce(proj2.transform.forward * 800);
            proj.tag = Tags.PlayerProjectile;
            proj2.tag = Tags.PlayerProjectile;
        }
        else if (get(Stats.scattershot).value == 3.0)
        {
            Projectile proj = projectileManager.getProjectile(PType.ARROW);
            proj.gameObject.tag = Tags.EnemyProjectile;
            proj.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
            proj.transform.rotation = transform.rotation;
            proj.transform.RotateAround(transform.position, transform.up, 300f);
            //proj.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 2.0f, 0.0f));
            //proj.transform.position += transform.right * 0.7f;


            Projectile proj2 = projectileManager.getProjectile(PType.ARROW);
            proj2.gameObject.tag = Tags.EnemyProjectile;
            proj2.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
            proj2.transform.rotation = transform.rotation;
            proj.transform.RotateAround(transform.position, transform.up, 180f);

            Projectile proj3 = projectileManager.getProjectile(PType.ARROW);
            proj3.gameObject.tag = Tags.EnemyProjectile;
            proj3.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
            proj3.transform.rotation = transform.rotation;
            //proj.transform.RotateAround(transform.position, transform.up, -45f);

            proj.damage = damage;
            proj2.damage = damage;
            proj3.damage = damage;

            proj.rb.AddForce(proj.transform.forward * 800);
            proj2.rb.AddForce(proj2.transform.forward * 800);
            proj3.rb.AddForce(proj3.transform.forward * 800);

            proj.tag = Tags.PlayerProjectile;
            proj2.tag = Tags.PlayerProjectile;
            proj3.tag = Tags.PlayerProjectile;
        }
    }

    public void fireProjectile(PType type, int num)
    {
        // calculate damage dealt
        for (int i =0; i< num;i++)
        {
            float damage = get(Stats.attack).value;

            Projectile p;
            p = projectileManager.getProjectile(type);
            p.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
            p.transform.rotation = transform.rotation;

            //crit chance
            p.damage = damage;
            if (Random.value < get(Stats.critChance).value)
            {
                p.damage += damage;
            }
            p.pierce = get(Stats.pierce).value;
            p.rb.AddForce(transform.forward * get(Stats.shotSpeed).value * 100.0f);

            p.gameObject.tag = Tags.PlayerProjectile;
            
        }
        


    }

}
