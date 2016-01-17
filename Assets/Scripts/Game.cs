using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

    public int level = 1;

    private StatUI ui;
    private SpawnManager spawner;
    private Player player;
    private Text frontInd;
    private Text backInd;


    private bool betweenLevels = false;

    // Use this for initialization
    void Start() {
        ui = GameObject.Find("StatUI").GetComponent<StatUI>();
        spawner = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        frontInd = transform.Find("LevelIndicator").GetComponent<Text>();
        backInd = transform.Find("LevelIndicatorBack").GetComponent<Text>();

        spawnGuys();
    }

    // Update is called once per frame
    void Update() {
        if (spawner.activeEnemies <= 0 && !betweenLevels && !spawner.spawning) {
            betweenLevels = true;
            StartCoroutine(loadLevelSequence());
        }
    }

    private void spawnGuys() {
        int number = level * 10;

        for (int i = 0; i < number; i++) {
            float rnd = Random.value;
            if (rnd < 0.3f) {
                spawner.BuildEnemy(EnemyType.SKELETON);
            } else if (rnd < 0.5f) {
                spawner.BuildEnemy(EnemyType.RANGER);
            } else if (rnd < 0.70f) {
                spawner.BuildEnemy(EnemyType.CROSSBOW);
            } else if (rnd < 0.90f) {
                spawner.BuildEnemy(EnemyType.MAGE);
            } else {
                spawner.BuildEnemy(EnemyType.ORC);
            }
        }

        spawner.spawning = true;
    }

    private IEnumerator loadLevelSequence() {
        player.freezeUpdate = true;

        frontInd.enabled = true;
        frontInd.text = "Level " + level + " defeated!";
        frontInd.color = Color.red;
        backInd.enabled = true;
        backInd.text = "Level " + level + " defeated!";
        backInd.color = Color.black;
        // play sound or something?

        yield return new WaitForSeconds(3.0f);

        frontInd.enabled = false;
        backInd.enabled = false;

        ui.buildUI(level);

        // wait for ui stat upgrading to finish
        while (ui.visible) {
            yield return null;
        }

        // next level!
        level++;
        frontInd.enabled = true;
        frontInd.text = "Level " + level;
        frontInd.color = Color.white;
        backInd.enabled = true;
        backInd.text = "Level " + level;
        StartCoroutine(fadeOutText());

        player.transform.position = Vector3.zero;
        player.freezeUpdate = false;
        betweenLevels = false;
        spawnGuys();
    }

    private IEnumerator fadeOutText() {
        yield return new WaitForSeconds(2.0f);
        float t = 3.0f;
        while (t > 0.0f) {
            t -= Time.deltaTime;
            frontInd.color = new Color(1.0f, 1.0f, 1.0f, t / 2.0f);
            backInd.color = new Color(0.0f, 0.0f, 0.0f, t / 2.0f);
            yield return null;
        }
        frontInd.enabled = false;
        backInd.enabled = false;
    }
}
