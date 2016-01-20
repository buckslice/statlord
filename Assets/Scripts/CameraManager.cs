using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
    public UnityStandardAssets.ImageEffects.Bloom bloom;
    public float shake = 0;
    public float shakeAmount = 0.7f;
    public float shakeDecreaseFactor = 1.0f;

    private PlayerStats stats;
    private Player player;
    private Vector3 camStart;

    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        camStart = transform.parent.position;
    }

    // Update is called once per frame
    void Update() {
        setBloom();

        // set camera to follow player
        transform.parent.position = camStart + player.transform.position;

        // camera shake
        if (shake > 0) {
            transform.localPosition = Random.insideUnitSphere * shake;
            shake -= Time.deltaTime * shakeDecreaseFactor * (shake + 1.0f);
        } else {
            transform.localPosition = Vector3.zero;
            shake = 0.0f;
        }
    }

    public void addShake(float scale) {
        shake += stats.get(Stats.cameraShake).value * scale * 0.25f;
    }

    public void setBloom() {
        bloom.bloomIntensity = stats.get(Stats.bloom).value;
    }
}
