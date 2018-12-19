using UnityEngine;

[DisallowMultipleComponent]
public class ForceManaged : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public GameObject parent;
}