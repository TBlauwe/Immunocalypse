using UnityEngine;

[RequireComponent(typeof(WithHealth))]
public class Eater : MonoBehaviour {
    public float eatDelta;
    [HideInInspector] public float cooldown;
    public float eatingLimitBeforeDeath;
    public int eatMask;
}