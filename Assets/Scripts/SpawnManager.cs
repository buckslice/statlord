using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnManager : MonoBehaviour {
    public GameObject Skeleton, Ranger, Mage, CrossBow, Orc;
    //private PlayerStats player;
    public int activeEnemies = 0;
    private List<GameObject> pool;
    private GameObject[] spawnLocations;
    public bool spawning { get; set; }

    void Awake() {
        //player = GameObject.Find("Player").GetComponent<PlayerStats>();
        spawnLocations = GameObject.FindGameObjectsWithTag("SpawnPoint");
        pool = new List<GameObject>();
        spawning = false;
    }

    public void BuildEnemy(EnemyType type) {
        GameObject x;
        switch (type) {
            case EnemyType.SKELETON:
                x = Instantiate(Skeleton, Vector3.zero, Quaternion.identity) as GameObject;
                break;
            case EnemyType.RANGER:
                x = Instantiate(Ranger, Vector3.zero, Quaternion.identity) as GameObject;
                break;
            case EnemyType.CROSSBOW:
                x = Instantiate(CrossBow, Vector3.zero, Quaternion.identity) as GameObject;
                break;
            case EnemyType.MAGE:
                x = Instantiate(Mage, Vector3.zero, Quaternion.identity) as GameObject;
                break;
            case EnemyType.ORC:
                x = Instantiate(Orc, Vector3.zero, Quaternion.identity) as GameObject;
                break;
            default:
                x = Instantiate(Skeleton, Vector3.zero, Quaternion.identity) as GameObject;
                break;
        }

        x.GetComponent<EnemyBasicScript>().initialize(this);
        x.transform.parent = transform;
        x.gameObject.tag = Tags.Enemy;
        x.SetActive(false);
        pool.Add(x);
        activeEnemies++;
    }

    float spawnTime = 0.0f;

    // Update is called once per frame
    void Update() {
        if (!spawning) {
            return;
        }
        spawnTime -= Time.deltaTime;
        if (spawnTime < 0.0f) {
            spawnTime = 1.25f;
            if (pool.Count > 0) {
                int last = pool.Count - 1;
                GameObject enemy = pool[last];
                pool.RemoveAt(last);

                enemy.SetActive(true);
                enemy.GetComponent<EnemyBasicScript>().hp = 3;

                int rng = Random.Range(0, spawnLocations.Length);
                enemy.transform.position = spawnLocations[rng].transform.position;
            } else {
                spawning = false;
            }
        }
    }

    public void killAll() {
        spawning = false;
        activeEnemies = 0;
        while(pool.Count > 0) {
            GameObject go = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            Destroy(go);
        }
        for (int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void notifyDeath() {
        activeEnemies--;
    }

    //public void returnEnemy(GameObject deadEnemy) {

    //    deadEnemy.gameObject.SetActive(false);
    //    pool.Add(deadEnemy);
    //}
}
