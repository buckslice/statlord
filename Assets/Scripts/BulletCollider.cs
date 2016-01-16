using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

    public GameObject bulletModel; 
    public float damage;
    void Start()
    {
        Destroy(bulletModel, 3);
    }
    void OnCollisionEnter()

    {
        Destroy(bulletModel);
    }

    public void setDamage(float playerDamage)
    {
        damage = playerDamage;
    }

    public float getDamage()
    {
        return damage;
    }
    
	
}
