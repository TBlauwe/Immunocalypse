using UnityEngine;

[RequireComponent(typeof(Eater), typeof(Movable))]
public class Macrophage : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public GameObject targetDetectionRange;
    public GameObject eatingRange;
}