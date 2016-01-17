using UnityEngine;
using System.Collections;


public class EnemyBasicScript : MonoBehaviour {
    public int hp;
    private SpawnManager manager;

    public void initialize(SpawnManager manager) {
        this.manager = manager;


        reset();
    }

    public void reset() {
        hp = 5;
    }

    // Update is called once per frame
    void Update() {

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
