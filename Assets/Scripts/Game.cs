using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

    public int level = 1;

    private StatUI ui;
    private SpawnManager spawner;
    private ProjectileManager shooter;
    private Player player;
    private Text frontInd;
    private Text backInd;

    private GameObject overlay;
    private RawImage fadeImage;

    private bool loading = true;

    // Use this for initialization
    void Start() {
        ui = GameObject.Find("StatUI").GetComponent<StatUI>();
        spawner = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        shooter = GameObject.Find("ProjectileManager").GetComponent<ProjectileManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        frontInd = transform.Find("LevelIndicator").GetComponent<Text>();
        backInd = transform.Find("LevelIndicatorBack").GetComponent<Text>();
        overlay = transform.Find("Overlay").gameObject;
        fadeImage = overlay.GetComponent<RawImage>();

        player.reset();

        StartCoroutine(startSequence());
    }

    // Update is called once per frame
    void Update() {
        if (loading) {
            return;
        }

        if (spawner.enemies <= 0) {
            loading = true;
            StartCoroutine(loadLevelSequence());
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            loading = true;
            StartCoroutine(quitToMenu());
        }
    }

    private float[] spawnGrowth = { 0.01f, 0.04f, 0.05f, 0.03f };
    private float[] spawnMax = { 0.1f, 0.2f, 0.15f, 0.2f };

    private void loadSpawner() {
        float levelf = level;
        int number = 10 + (level - 1) * 3;
        spawner.spawnInterval = 1.5f - levelf * 0.02f;

        //int number = 100;
        //spawner.spawnInterval = 0.2f;

        for (int i = 0; i < number; i++) {
            float rnd = Random.value;

            float bas = 0.0f;
            for (int j = 0; j < 5; j++) {
                if(j == 4) {
                    spawner.BuildEnemy(EnemyType.SKELETON);
                    break;
                }
                float chance = Mathf.Min(levelf * spawnGrowth[j], spawnMax[j]);
                if (chance + bas > rnd) {
                    switch (j) {
                        case 0:
                            spawner.BuildEnemy(EnemyType.ORC);
                            break;
                        case 1:
                            spawner.BuildEnemy(EnemyType.MAGE);
                            break;
                        case 2:
                            spawner.BuildEnemy(EnemyType.RANGER);
                            break;
                        case 3:
                            spawner.BuildEnemy(EnemyType.CROSSBOW);
                            break;
                        default:
                            break;
                    }
                    break;
                }
                bas += chance;
            }
        }

    }

    private IEnumerator loadLevelSequence() {
        player.freezeUpdate = true;

        frontInd.enabled = true;
        backInd.enabled = true;
        frontInd.text = "Level " + level + " Defeated!";
        backInd.text = "Level " + level + " Defeated!";
        frontInd.color = Color.green;
        backInd.color = Color.black;
        // play sound or something?

        yield return new WaitForSeconds(2.0f);

        frontInd.enabled = false;
        backInd.enabled = false;
        player.setHealthBar(false);

        // if just beat level 16 then you win
        // 16 is the first level with all stats unlocked
        if (level > 15) {
            SceneManager.LoadScene(2);
            yield break;
        }

        ui.buildUI(level);

        // wait for ui stat upgrading to finish
        while (ui.visible) {
            yield return null;
        }

        // next level!
        level++;

        player.setHealthBar(true);
        player.reset();
        player.freezeUpdate = false;

        yield return startSequence();
    }

    public void restartLevel() {
        if (loading) {
            return;
        }
        loading = true;
        StartCoroutine(deathSequence());
    }

    private IEnumerator deathSequence() {
        player.freezeUpdate = true;

        frontInd.enabled = true;
        backInd.enabled = true;
        frontInd.text = "A GLORIOUS DEATH!";
        backInd.text = "A GLORIOUS DEATH!";
        frontInd.color = Color.red;
        backInd.color = Color.black;
        StartCoroutine(fadeOutText(2.0f, 1.0f));
        spawner.destroyHealthbars();

        yield return fade(false, 3.0f);
        frontInd.enabled = false;
        backInd.enabled = false;
        spawner.killAll();
        shooter.returnAll();
        yield return new WaitForSeconds(1.0f);

        player.reset();
        player.freezeUpdate = false;

        yield return startSequence();
    }

    // loads level
    private IEnumerator startSequence() {
        frontInd.enabled = true;
        backInd.enabled = true;
        frontInd.text = "Level " + level;
        backInd.text = "Level " + level;
        frontInd.color = Color.white;
        backInd.color = Color.black;
        yield return fade(true, 1.0f);
        StartCoroutine(fadeOutText(1.0f, 2.0f));
        loadSpawner();
        loading = false;
    }

    // fades in from whatever color the image is at
    private IEnumerator fade(bool fadein, float time) {
        if (!fadein) {
            overlay.SetActive(true);
        }
        float t = time;
        while (t > 0.0f) {
            Color c = fadeImage.color;
            if (fadein) {
                c.a = t / time;
            } else {
                c.a = 1.0f - t / time;
            }
            fadeImage.color = c;
            t -= Time.deltaTime;
            yield return null;
        }
        // reset fade variables back to defaults
        fadeImage.color = Color.black;
        if (fadein) {
            overlay.SetActive(false);
        }
    }

    private IEnumerator fadeOutText(float delay, float speed) {
        yield return new WaitForSeconds(delay);
        Color cf = frontInd.color;
        Color cb = backInd.color;
        float t = speed;
        while (t > 0.0f) {
            t -= Time.deltaTime;
            frontInd.color = new Color(cf.r, cf.g, cf.b, t / speed);
            backInd.color = new Color(cb.r, cb.g, cb.b, t / speed);
            yield return null;
        }
        frontInd.enabled = false;
        backInd.enabled = false;
    }

    private IEnumerator quitToMenu() {
        yield return fade(false, 1.0f);
        SceneManager.LoadScene(0);
    }
}
