using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

    public GameObject bulletModel; 

    void Start()
    {
        Destroy(bulletModel, 3);
    }
    void OnCollisionEnter()

    {
        Destroy(bulletModel);
    }
    
	
}
