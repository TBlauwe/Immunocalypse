using UnityEngine;

public class Eater : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public int eatingMask;
    [Range(0.01f, 1)] public float eatingProbability;
}