using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private Transform tform;
    private Rigidbody myRigidbody;
    public bool grounded = true;
    //public float timeSinceJump = 0.0f;
    private PlayerStats stats;
    private Transform cam;

    // Use this for initialization
    void Start() {
        tform = transform;
        myRigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<PlayerStats>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update() {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.zero;
        if (inputX != 0.0f || inputY != 0.0f) {
            dir = cam.TransformDirection(new Vector3(inputX, 0.0f, inputY));
            dir.y = 0.0f;
            dir.Normalize();
        }
        dir *= stats.get(Stats.moveSpeed).value;

        float newY = myRigidbody.velocity.y;

        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            newY = stats.get(Stats.jumpSpeed).value;
            grounded = false;
        }

        myRigidbody.velocity = new Vector3(dir.x, newY, dir.z);
    }

    void FixedUpdate() {
        Vector3 castStart = tform.position + new Vector3(0.0f, 0.5f, 0.0f);
        RaycastHit info;
        grounded = Physics.SphereCast(castStart, 0.25f, Vector3.down, out info, 0.5f);
        if(myRigidbody.velocity.y > 0.1f) { // prevent double jumping
           grounded = false;
        }
    }

}
