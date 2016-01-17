using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    int level = 1;

    StatUI ui;

	// Use this for initialization
	void Start () {
        ui = GameObject.Find("StatUI").GetComponent<StatUI>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
