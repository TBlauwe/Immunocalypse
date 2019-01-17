using UnityEngine;

[RequireComponent(typeof(Origin))]
public class Bacteria : MonoBehaviour {
    public float replicationDelay = 20;
    [HideInInspector] public float replicationCooldown;
}