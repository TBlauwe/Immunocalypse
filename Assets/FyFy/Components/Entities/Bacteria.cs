using UnityEngine;

[RequireComponent(typeof(Origin))]
public class Bacteria : MonoBehaviour {
    public float replicationDelay = 20;
    public GameObject target;
    [HideInInspector] public float replicationCooldown;
}