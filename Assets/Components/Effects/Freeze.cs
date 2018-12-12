using UnityEngine;

public class Freeze : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    [HideInInspector] public float remaining;
}