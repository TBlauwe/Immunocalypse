using UnityEngine;

public class Factory : MonoBehaviour {
    public GameObject[] prefabs;
    [Range(0, 1)] public float[] probabilities;
    public float rate;
    [Range(1, 10)] public int numberOfWaves;
    [HideInInspector] public float remainingTime;
    public int individualPerWave;
}