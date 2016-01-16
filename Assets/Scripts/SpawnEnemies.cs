using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemies : MonoBehaviour {
    public GameObject enemy;
    private GameObject player;
    public float startTime, spawnRate;
    public int numOfEnemies;
    private int playerLevel;
    private List<GameObject> enabledEnemies, disabledEnemies;
    private GameObject[] spawnLocations;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLevel = player.GetComponent<PlayerStats>().level;
        spawnLocations = GameObject.FindGameObjectsWithTag("SpawnPoint");
        disabledEnemies = new List<GameObject>();
        enabledEnemies = new List<GameObject>();
    }
	// Use this for initialization
	void Start () {
        for (int i = 0; i <= numOfEnemies;i++ )
        {
            SpawnEnemy();
        }
        InvokeRepeating("EnableEnemy", startTime, spawnRate);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void SpawnEnemy()
    {
        int rng = Random.Range(0, spawnLocations.Length);
        GameObject x = Instantiate(enemy, spawnLocations[rng].transform.position, transform.rotation) as GameObject;
        
        x.SetActive(false);
        disabledEnemies.Add(x);
    }
    void EnableEnemy()
    {
        try
        {
            disabledEnemies[0].SetActive(true);
            disabledEnemies[0].GetComponent<EnemyBasicScript>().SetHP(playerLevel);
            //enabledEnemies.Add(disabledEnemies[0]);
            disabledEnemies.Remove(disabledEnemies[0]);
            
        }
        catch
        {
            CancelInvoke();
        }
    }

    public void addToEnemyPool(GameObject deadEnemy)
    {
        disabledEnemies.Add(deadEnemy);
    }
}
