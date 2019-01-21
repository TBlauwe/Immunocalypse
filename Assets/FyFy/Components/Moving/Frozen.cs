using UnityEngine;

public class Frozen : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    [HideInInspector] public float remainingTime;
    [HideInInspector] public float totalTime;
}