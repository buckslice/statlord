using UnityEngine;
using System.Collections;

public class HellFire : MonoBehaviour {
    public GameObject fireBall;
    public GameObject spawner;
    public float nextFire = 0.0f;
    public float fireRate = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //if (Time.time > nextFire)
        //{
        //    nextFire = Time.time + fireRate;
        //    Instantiate(fireBall,spawner.transform.position , Quaternion.identity);
        //}

    }
}
