using UnityEngine;
using System.Collections;


public class EnemyBasicScript : MonoBehaviour {
    public int hp;
    private GameObject enemySpawner;
	// Use this for initialization
	void Awake () {
        hp = 5;
        enemySpawner = GameObject.Find("Spawner");
	}
	
	// Update is called once per frame
	void Update () 
    {

	    if (hp<=0)
        {
            enemySpawner.GetComponent<SpawnManager>().addToEnemyPool(gameObject);
            gameObject.SetActive(false);
        }
	}

    void OnColissionEnter(Collision collider)
    {
        //collider.gameObject.GetComponent<
    }

    public void SetHP(int playerLevel)
        //set the enemy health to be an amount of the playerlevel. this one is a direct player level * 3
    {
        hp = playerLevel * 3;
    }

    public void SetHP(int playerLevel, int number)
    //set the enemy health to be "number" * playerLevel
    {
        hp = playerLevel * number;
    }

    public void SetHP(int playerLevel, float number)
    //set the enemy health to be "number" * playerLevel. 
    //This allows for floating numbers to be multiplied. 
    //aka player level * 1.5
    {
        hp = Mathf.RoundToInt(playerLevel * number);
    }




}
