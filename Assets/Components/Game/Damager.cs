using FYFY_plugins.TriggerManager;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damager : MonoBehaviour {
    public float attackPoints = 3;
    public int damagablesLayerMask = 0;
}