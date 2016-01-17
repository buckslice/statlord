using UnityEngine;
using System.Collections;

public class EnemyBasicScript : MonoBehaviour {
    public int hp;
    private SpawnManager manager;
    Transform player; // reference to the player position 
    NavMeshAgent nav; // reference to the  nav mesh agent used to move locate and move towards player 

    public void initialize(SpawnManager manager) {
        this.manager = manager;
        player = GameObject.Find("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        reset();
    }

    public void reset() {
        hp = 5;
    }

    // Update is called once per frame
    void Update() {
        nav.SetDestination(player.position);
        if (hp <= 0) {
            manager.returnEnemy(gameObject);
        }
    }

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == Tags.PlayerProjectile) {
            manager.returnEnemy(gameObject);
        }
    }
}
