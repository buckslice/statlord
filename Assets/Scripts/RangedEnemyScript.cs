using UnityEngine;
using System.Collections;

public class RangedEnemyScript : MonoBehaviour {
    private float timeBetweenShot = 3.0f;
    private Transform player;
    private ProjectileManager projMan;
    private EnemyBasicScript ebs;

    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player").transform;
        projMan = GameObject.Find("ProjectileManager").GetComponent<ProjectileManager>();
        ebs = GetComponent<EnemyBasicScript>();
    }

    // Update is called once per frame
    void Update() {
        timeBetweenShot -= Time.deltaTime;

        if ((timeBetweenShot <= 0.0f) && (!ebs.frozen)) {

            switch (ebs.type) {
                case EnemyType.RANGER:
                    FireArrow();
                    break;
                case EnemyType.CROSSBOW:
                    FireTwinArrow();
                    break;
                case EnemyType.MAGE:
                    FireFireball();
                    break;
                default:
                    break;
            }

            timeBetweenShot = 3.0f;
        }
    }

    private Quaternion getPoorlyAimedRotation() {
        Vector2 rnd = Random.insideUnitCircle * 8.0f;
        Vector3 target = player.position + new Vector3(rnd.x, 0.0f, rnd.y);
        target.y = 0.0f;
        return Quaternion.LookRotation(target - transform.position, Vector3.up);
    }

    void FireTwinArrow() {
        Projectile proj = projMan.getProjectile(PType.ARROW);
        proj.gameObject.tag = Tags.EnemyProjectile;
        proj.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
        proj.transform.position += transform.right * 0.7f;
        proj.transform.rotation = getPoorlyAimedRotation();
        proj.damage = Mathf.Max(4,ebs.damage);

        Projectile proj2 = projMan.getProjectile(PType.ARROW);
        proj2.gameObject.tag = Tags.EnemyProjectile;
        proj2.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
        proj2.transform.position -= transform.right * 0.7f;
        proj2.transform.rotation = getPoorlyAimedRotation();
        proj2.damage = proj.damage = Mathf.Max(4, ebs.damage); ;


        proj.rb.AddForce(proj.transform.forward * 800);
        proj2.rb.AddForce(proj2.transform.forward * 800);
        proj.tag = Tags.EnemyProjectile;
        proj2.tag = Tags.EnemyProjectile;
    }

    void FireArrow() {
        Projectile proj = projMan.getProjectile(PType.ARROW);
        proj.gameObject.tag = Tags.EnemyProjectile;
        proj.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
        proj.transform.rotation = getPoorlyAimedRotation();

        proj.damage = Mathf.Max(5, ebs.damage);

        proj.rb.AddForce(proj.transform.forward * 800);

        proj.tag = Tags.EnemyProjectile;
    }

    void FireFireball() {
        Projectile proj = projMan.getProjectile(PType.FIREBALL);
        proj.gameObject.tag = Tags.EnemyProjectile;
        proj.transform.position = transform.position + new Vector3(0, 1.0f, 0) + (transform.forward * 1.25f);
        proj.transform.rotation = getPoorlyAimedRotation();

        proj.damage = Mathf.Max(10, ebs.damage);

        proj.rb.AddForce(proj.transform.forward * 800);
        proj.tag = Tags.EnemyProjectile;
    }
}
