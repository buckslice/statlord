using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {
    public GameObject Skeleton, Ranger, Mage, CrossBowMan;
    private PlayerStats player;
    public float startTime, spawnRate;
    public int maxEnemies;
    private List<GameObject> pool;
    private GameObject[] spawnLocations;

    void Awake() {
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        spawnLocations = GameObject.FindGameObjectsWithTag("SpawnPoint");
        pool = new List<GameObject>();
    }
    // Use this for initialization
    void Start() {
        for (int i = 0; i <= maxEnemies; i++) {
            BuildEnemy();
        }
    }

    void BuildEnemy() {
        GameObject x;
        int rng = Random.Range(0, 25);
        if (rng <= 5) {
            x = Instantiate(Skeleton, Vector3.zero, Quaternion.identity) as GameObject;

        } else if (rng <= 10) {
            x = Instantiate(Ranger, Vector3.zero, Quaternion.identity) as GameObject;
        } else if (rng <= 20) {
            x = Instantiate(Mage, Vector3.zero, Quaternion.identity) as GameObject;
        } else {
            x = Instantiate(CrossBowMan, Vector3.zero, Quaternion.identity) as GameObject;
        }

        x.GetComponent<EnemyBasicScript>().initialize(this);
        x.transform.parent = transform;
        x.SetActive(false);
        x.gameObject.tag = Tags.Enemy;
        pool.Add(x);
    }

    float spawnTime = 0.0f;

    // Update is called once per frame
    void Update() {
        spawnTime -= Time.deltaTime;
        if (spawnTime < 0.0f) {
            spawnTime = spawnRate;
            if (pool.Count > 0) {
                int last = pool.Count - 1;
                GameObject enemy = pool[last];
                pool.RemoveAt(last);

                enemy.SetActive(true);
                enemy.GetComponent<EnemyBasicScript>().hp = 3;

                int rng = Random.Range(0, spawnLocations.Length);
                enemy.transform.position = spawnLocations[rng].transform.position;
            }
        }
    }

    public void returnEnemy(GameObject deadEnemy) {
        deadEnemy.gameObject.SetActive(false);
        pool.Add(deadEnemy);
    }
}
