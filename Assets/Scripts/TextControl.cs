using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TextControl : MonoBehaviour {

    private GameObject overlay;
    private RawImage fadeImage;
    private Material mat;
    void Start() {
        mat = gameObject.GetComponent<Renderer>().material;
        mat.color = Color.black;
        overlay = GameObject.Find("Canvas").transform.Find("Overlay").gameObject;
        fadeImage = overlay.GetComponent<RawImage>();

        loading = true;
        StartCoroutine(fade(true, 1.0f));
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    void OnMouseOver() {
        if (loading) {
            return;
        }
        mat.color = Color.red;
    }

    void OnMouseExit() {
        mat.color = Color.black;
    }

    private bool loading = false;
    void OnMouseDown() {
        if (!loading) {
            loading = true;
            StartCoroutine(fadeGo());
        }
    }

    private IEnumerator fadeGo() {
        yield return fade(false, 1.0f);
        SceneManager.LoadScene(1);
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
        loading = false;
    }

}
