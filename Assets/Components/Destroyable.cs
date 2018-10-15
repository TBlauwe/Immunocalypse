using UnityEngine;

[RequireComponent(typeof(WithHealth))]
public class Destroyable : MonoBehaviour {
    public float defensePoints = 1;
    public int layerMask = 0;
}