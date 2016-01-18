﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TextControl : MonoBehaviour {

    private GameObject overlay;
    private RawImage fadeImage;
    private Material mat;
    void Start() {
        mat = gameObject.GetComponent<Renderer>().material;
        mat.color = Color.blue;
        overlay = GameObject.Find("Canvas").transform.Find("Overlay").gameObject;
        fadeImage = overlay.GetComponent<RawImage>();

        StartCoroutine(fade(true));
    }

    void OnMouseEnter() { 
        mat.color = Color.magenta;
    }

    void OnMouseExit() {
        mat.color = Color.blue;
    }

    private bool loading = false;
    void OnMouseDown() {
        if (!loading) {
            loading = true;
            StartCoroutine(fadeGo());
        }
    }

    private IEnumerator fadeGo() {
        yield return fade(false);
        SceneManager.LoadScene(1);
    }

    float fadeTime = 2.0f;
    // fades in from whatever color the image is at
    private IEnumerator fade(bool fadein) {
        if (!fadein) {
            overlay.SetActive(true);
        }
        float t = fadeTime;
        while (t > 0.0f) {
            Color c = fadeImage.color;
            if (fadein) {
                c.a = t / fadeTime;
            } else {
                c.a = 1.0f - t / fadeTime;
            }
            fadeImage.color = c;
            t -= Time.deltaTime;
            yield return null;
        }
        // reset fade variables back to defaults
        fadeImage.color = Color.black;
        fadeTime = 2.0f;
        if (fadein) {
            overlay.SetActive(false);
        }
    }

}
