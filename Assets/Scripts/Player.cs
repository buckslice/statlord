using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    Vector3 dir;
    private Rigidbody rigidbody;
    public float MoveSpeed;
    public float jumpForce;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.A)))
        { //top left
            dir = Vector3.forward;
            dir += -Vector3.right;
            rigidbody.velocity = dir * MoveSpeed;
        }

        else if (Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.D)))
        { //top right
            dir = Vector3.forward;
            dir += Vector3.right;
            rigidbody.velocity = dir * MoveSpeed;
        }

        else if (Input.GetKey(KeyCode.S) && (Input.GetKey(KeyCode.D)))
        { //bottom right
            dir = -Vector3.forward;
            dir += Vector3.right;

            rigidbody.velocity = dir * MoveSpeed;
        }
        else if (Input.GetKey(KeyCode.S) && (Input.GetKey(KeyCode.A)))
        { //bottom right
            dir = -Vector3.forward;
            dir += -Vector3.right;
            rigidbody.velocity = dir * MoveSpeed;
        }

        else if (Input.GetKey(KeyCode.D))
        {

            dir = Vector3.right;
            rigidbody.velocity = dir * MoveSpeed;

        }
        else if (Input.GetKey(KeyCode.S))
        {

            dir = -Vector3.forward;    // '-up' means 'down'
            rigidbody.velocity = dir * MoveSpeed;

        }
        else if (Input.GetKey(KeyCode.A))
        {

            dir = -Vector3.right; // '-right' means 'left'
            rigidbody.velocity = dir * MoveSpeed;

        }
        else if (Input.GetKey(KeyCode.W))
        {

            dir = Vector3.forward;
            rigidbody.velocity = dir * MoveSpeed;

        }
        else if (Input.GetKey(KeyCode.Space))
        {
            dir = Vector3.up;
            rigidbody.velocity = dir * jumpForce;
        }
        else
        {
            rigidbody.velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
	}
}
