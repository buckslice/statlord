using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private Transform tform;
    private Rigidbody myRigidbody;
    public bool freezeUpdate = false;
    public bool grounded = true;
    private PlayerStats stats;
    private CameraManager cam;

    // timers
    private float timeSinceAttack = 0.0f;
    private float curHealth = 1.0f;

    // Use this for initialization
    void Start() {
        tform = transform;
        myRigidbody = GetComponent<Rigidbody>();
        stats = GetComponent<PlayerStats>();
        cam = Camera.main.GetComponent<CameraManager>();
        Application.runInBackground = true;
    }

    // Update is called once per frame
    void Update() {

        // find where on ground plane your mouse is pointing and look there
        RaycastHit info;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out info, 1000.0f, 1 << 8)) {
            Vector3 fw = info.point - tform.position;
            fw.y = 0.0f;
            fw.Normalize();
            transform.rotation = Quaternion.LookRotation(fw);
        }

        // so you can still look around at least
        if (freezeUpdate) {
            myRigidbody.velocity = Vector3.zero;
            return;
        }

        // calculate movement based on camera view
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector3 dir = Vector3.zero;
        if (inputX != 0.0f || inputY != 0.0f) {
            dir = cam.transform.TransformDirection(new Vector3(inputX, 0.0f, inputY));
            dir.y = 0.0f;
            dir.Normalize();
        }
        dir *= stats.get(Stats.moveSpeed).value;

        // jumping code
        float newY = myRigidbody.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            newY = stats.get(Stats.jumpSpeed).value;
            grounded = false;
        }
        myRigidbody.velocity = new Vector3(dir.x, newY, dir.z);

        // attack if holding down mouse
        timeSinceAttack -= Time.deltaTime;
        if (Input.GetMouseButton(0) && timeSinceAttack < 0.0f) {
            timeSinceAttack = stats.get(Stats.attackRate).value;
            if (Random.value < stats.get(Stats.fireballChance).value) {
                stats.fireProjectile(PType.FIREBALL);
            } else {
                stats.fireProjectile(PType.ARROW);
            }

        }
        //else if (Input.GetMouseButton(1) && timeSinceAttack < 0.0f) {
        //    timeSinceAttack = stats.get(Stats.attackRate).value;
        //    stats.fireProjectile(PType.FIREBALL);
        //}

        // die if no health
        if (curHealth <= 0.0f) {
            // do something here eventually

        }

    }

    public void reset() {
        transform.position = Vector3.zero;
        curHealth = stats.get(Stats.health).value;
    }

    void OnTriggerEnter(Collider c) {
        if ((c.gameObject.tag == Tags.EnemyProjectile) || (c.gameObject.tag == Tags.Enemy)) {
            curHealth -= 1.0f;
            cam.addShake();
        }
    }

    void FixedUpdate() {
        // check to see if player is grounded
        Vector3 castStart = tform.position + new Vector3(0.0f, 0.5f, 0.0f);
        RaycastHit info;
        grounded = Physics.SphereCast(castStart, 0.25f, Vector3.down, out info, 0.5f);
        if (myRigidbody.velocity.y > 0.1f) { // prevent double jumping
            grounded = false;
        }
    }

}
