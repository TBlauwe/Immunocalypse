using UnityEngine;

[RequireComponent(typeof(Origin))]
public class Virus : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public GameObject target;
}