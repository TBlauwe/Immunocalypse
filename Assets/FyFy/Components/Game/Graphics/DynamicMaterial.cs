using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WithHealth))]
public class DynamicMaterial : MonoBehaviour {
    public float  percentage=1;     // Used to determine which materials should be changed
    public string materialName;     // Used to determine which materials should be changed
    public Color startingColor;     // Explicit
    public Color endingColor;       // Explicit
}