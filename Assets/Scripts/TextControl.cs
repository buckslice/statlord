using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TextControl : MonoBehaviour {


void OnMouseEnter()
    {

        gameObject.GetComponent<Renderer>().material.color = Color.red;

    }

void OnMouseExit()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
void OnMouseDown()
    {
        SceneManager.LoadScene(1);
        
    }

}
