using UnityEngine;
using System.Collections;

public class TravelingMusic : MonoBehaviour {

    public static bool created = false;

    void Awake() {
        if (created) {
            Destroy(gameObject);
        } else {
            created = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}
