using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public UnityStandardAssets.ImageEffects.Bloom bloom;

    private PlayerStats stats;

    // Use this for initialization
    void Start()
    {
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        setBloom();
    }

    public void setBloom()
    {
        bloom.bloomIntensity = stats.get(Stats.increaseBloom).value;
    }
}
