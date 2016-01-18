using UnityEngine;
using System.Collections;

public enum EnemyType {
    SKELETON,
    RANGER,
    CROSSBOW,
    MAGE,
    ORC
};

public class EnemyBasicScript : MonoBehaviour {
    public EnemyType type;
    public float hp { get; set; }
    private SpawnManager manager;
    Transform player; // reference to the player position 
    NavMeshAgent nav; // reference to the  nav mesh agent used to move locate and move towards player 
    private bool dying = false;
    private Game game;
    private RectTransform canvas;
    public bool frozen;
    public float damage;
    // "animation" variables
    private Transform model;
    float yoff = 0.0f;
    float timer = 0.0f;
    float xx = 0.0f;


    public void Start() {
        canvas = GameObject.Find("StatUI").GetComponent<RectTransform>();
        game = canvas.GetComponent<Game>();
        switch (type) {
            case EnemyType.ORC:
                hp = 2.0f + game.level;
                damage = 2.0f + game.level;
                break;
            case EnemyType.SKELETON:
                hp = 1.0f;
                damage = 1.0f + (game.level) / 5.0f;
                break;
            case EnemyType.MAGE:
                hp = 1.0f + game.level / 2.0f;
                damage = 3.0f + game.level / 2.0f;
                break;
            case EnemyType.RANGER:
                hp = 1.0f + game.level / 2.0f;
                damage = 1.0f + game.level / 2.0f;
                break;
            case EnemyType.CROSSBOW:
                hp = 1.0f + game.level / 2.0f;
                damage = 1.0f + game.level / 2.0f;
                break;
            default:
                hp = 1.0f + game.level / 2.0f;
                damage = 1.0f + game.level / 2.0f;
                break;
        }
    }

    public void Awake()
    {
        frozen = false;
    }

    public void initialize(SpawnManager manager) {
        this.manager = manager;
        model = transform.Find("Model").transform;
        player = GameObject.Find("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {

        if (dying) {
            return;
        }

        // look at player if within stopping distance
        float sqrmag = Vector3.SqrMagnitude(player.position - transform.position);
        if (sqrmag < nav.stoppingDistance * nav.stoppingDistance) {
            Quaternion targ = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targ, Time.deltaTime * 2.0f);
        }

        // bob up and down and side to side
        if (nav.velocity.sqrMagnitude < 5.0f) {
            timer = -3.14159f / 2.0f;  // sin of this is -1
            yoff -= Time.deltaTime * 2.0f;
            if (yoff < 0.0f) {
                yoff = 0.0f;
            }
            xx *= .9f;
        } else {
            timer += Time.deltaTime * 10.0f;
            yoff = (Mathf.Sin(timer) + 1.0f) * .25f;
            xx = Mathf.Sin(timer / 2.0f);
        }
        // up and down
        Vector3 mp = model.position;
        model.position = new Vector3(mp.x, yoff, mp.z);

        // side to side
        Vector3 newup = Vector3.up + Vector3.right * xx * .25f;
        model.localRotation = Quaternion.LookRotation(Vector3.forward, newup);
        if (frozen)
        {
            nav.ResetPath();
        }
        else
        {
            nav.SetDestination(player.position);
        }
        
        if (hp <= 0) {
            StartCoroutine(fallOverThenDie());
            dying = true;
            //manager.returnEnemy(gameObject);
        }
    }


    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == Tags.PlayerProjectile) {

            hp -= player.GetComponent<PlayerStats>().get(Stats.attack).value;

            if (dying) {
                //health gain on enemy kills
                player.GetComponent<Player>().curHealth += player.GetComponent<PlayerStats>().get(Stats.healthOnKill).value;
                StartCoroutine(fallOverThenDie());
                dying = true;
            }

            //manager.returnEnemy(gameObject);
        }

    }

    // fall over then die coroutine
    private IEnumerator fallOverThenDie() {
        manager.notifyDeath();

        // destroy logic processes
        Destroy(nav);
        RangedEnemyScript res = GetComponent<RangedEnemyScript>();
        if (res) {
            Destroy(res);
        }
        Destroy(GetComponent<Collider>());

        //fall over
        Vector3 newup = Random.onUnitSphere;
        newup.y = 0.0f;
        newup = newup.normalized;
        if (newup == Vector3.zero) {
            newup = Vector3.right;
        }
        Quaternion targetRot = Quaternion.AngleAxis(90, newup);
        Vector3 cur = transform.rotation.eulerAngles;
        Vector3 nur = targetRot.eulerAngles;
        targetRot = Quaternion.Euler(nur.x, cur.y, nur.z);

        float t = 0.8f;
        while (t > 0.0f) {
            t -= Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 1.0f - t / 0.8f);
            yield return null;
        }

        // sink into ground
        t = 4.0f;
        Vector3 p = transform.position;
        while (t > 0.0f) {
            t -= Time.deltaTime;
            transform.position = new Vector3(p.x, p.y - 2.0f * (1.0f - t / 4.0f), p.z);
            yield return null;
        }

        Destroy(gameObject);
    }
}
