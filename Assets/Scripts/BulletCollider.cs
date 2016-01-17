using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

    public GameObject bulletModel;
    private BulletManager bulletManager;
    public float damage;
    void Start()
    {
        bulletManager = GameObject.Find("Player").GetComponent<BulletManager>();
        Invoke("turnOff", 3.0f);
        
    }
    void OnCollisionEnter()
    {
        turnOff();
    }

    void turnOff()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bulletManager.AddBullet(gameObject);
        gameObject.SetActive(false);
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
