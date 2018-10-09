using UnityEngine;

[RequireComponent(typeof(WithHealth))]
public class Eater : MonoBehaviour {
    public float cooldown;
    public float nextMeal;
    public float eatingLimitBeforeDeath;
    public SphereCollider eatingCollider;
    public int eatMask;
}