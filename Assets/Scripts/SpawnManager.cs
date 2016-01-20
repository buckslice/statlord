using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnManager : MonoBehaviour {
    public GameObject Skeleton, Ranger, Mage, CrossBow, Orc;
    //private PlayerStats player;
    public int enemies { get; private set; }
    public float spawnInterval { get; set; }
    private List<GameObject> pool;
    private GameObject[] spawnLocations;

    void Awake() {
        //player = GameObject.Find("Player").GetComponent<PlayerStats>();
        spawnLocations = GameObject.FindGameObjectsWithTag("SpawnPoint");
        pool = new List<GameObject>();
        spawnInterval = 1.0f;
        enemies = 0;
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
        enemies++;
    }

    float spawnTime = 0.0f;

    // Update is called once per frame
    void Update() {
        spawnTime -= Time.deltaTime;
        if (spawnTime < 0.0f) {
            spawnTime = spawnInterval;
            if (pool.Count > 0) {
                int last = pool.Count - 1;
                GameObject enemy = pool[last];
                pool.RemoveAt(last);

                enemy.SetActive(true);
                enemy.GetComponent<EnemyBasicScript>().hp = 3;

                Vector3 spawnPoint = spawnLocations[Random.Range(0, spawnLocations.Length)].transform.position;
                Vector2 rnd = Random.insideUnitCircle * 5.0f;
                spawnPoint.x += rnd.x;
                spawnPoint.z += rnd.y;
                spawnPoint.y = 0.01f;
                enemy.transform.position = spawnPoint;
            }
        }
    }

    public void destroyHealthbars() {
        pool.Clear();   // dont spawn anymore dudes either
        for (int i = transform.childCount - 1; i >= 0; i--) {
            transform.GetChild(i).GetComponent<EnemyBasicScript>().destroyHealthBars();
        }
    }

    public void killAll() {
        enemies = 0;
        while (pool.Count > 0) {
            GameObject go = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            Destroy(go);
        }
        for (int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void notifyDeath() {
        enemies--;
    }

}
