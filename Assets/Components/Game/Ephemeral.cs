using UnityEngine;

[RequireComponent(typeof(WithHealth))]
public class Ephemeral : MonoBehaviour {
    public float lifetime = 60;
    public bool degressiveHealth = false;
    [HideInInspector] public float elapsedTime = 0;
    [HideInInspector] public float startingHealth = 0;
}