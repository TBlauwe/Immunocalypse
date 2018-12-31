using UnityEngine;

public class Bacteria : MonoBehaviour {
    public float replicationDelay = 20;
    public string prefabKey;
    public GameObject target;
    [HideInInspector] public float replicationCooldown;
}