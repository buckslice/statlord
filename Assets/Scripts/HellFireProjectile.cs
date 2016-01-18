using UnityEngine;
using System.Collections;

public class HellFireProjectile : MonoBehaviour {
    public GameObject fireBall;
	// Use this for initialization
	void Start () {
       
	
	}
	
	// Update is called once per frame
	void Update () {

        Destroy(fireBall, 5);

	
	}
}
