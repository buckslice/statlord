using UnityEngine;
using System.Collections;

public class SimpleMobNav : MonoBehaviour {

    Transform player; // reference to the player position 
    NavMeshAgent nav; // reference to the  nav mesh agent used to move locate and move towards player 

    void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        nav = GetComponent<NavMeshAgent>();

    }


    void Update()
    {
        nav.SetDestination(player.position);
    }
}
