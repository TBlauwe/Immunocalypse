using UnityEngine;

[RequireComponent(typeof(Origin))]
public class Infectious : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    // public int mask;
    public float replicationTime; // Time to replicate the pathogene in the Cell
    public float replicationCost; // How many PV are consumed to create a new Pathogene
    [HideInInspector] public float cooldown;
}