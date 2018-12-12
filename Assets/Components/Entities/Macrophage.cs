using UnityEngine;

public class Macrophage : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public GameObject eatingRange;
    public int eatingMask;
    [Range(0, 1.0f)] public float eatingProbability;
    public float cooldown;
    [HideInInspector] public float remaining;
}