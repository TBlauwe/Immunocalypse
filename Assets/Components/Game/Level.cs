using UnityEngine;

public class Level : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public Wave[] waves;
    [HideInInspector]public float runningTime = 0;
}