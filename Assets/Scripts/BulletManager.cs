using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager: MonoBehaviour {

    public GameObject bullet;
    private List<GameObject> listOfBullets;
    

	void Awake()
    {
        listOfBullets = new List<GameObject>();
        
        for (int i = 0; i<100;i++)
        {
            GameObject x = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;

            x.SetActive(false);
            listOfBullets.Add(x);
        }
        
    }
    public BulletCollider EnableBullet()
    {
        listOfBullets[0].SetActive(true);
        GameObject bullet = listOfBullets[0];
        listOfBullets.Remove(listOfBullets[0]);
        return bullet.GetComponent<BulletCollider>();
        
    }
    public void AddBullet(GameObject bullet)
    {
        listOfBullets.Add(bullet);
    }



}
