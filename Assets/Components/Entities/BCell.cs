using UnityEngine;

public class BCell : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public GameObject attackRange;
    public float delay;
    public float effectDuration;
    [HideInInspector] public float remaining;
}