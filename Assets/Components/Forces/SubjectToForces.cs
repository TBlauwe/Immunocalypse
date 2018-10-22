using UnityEngine;
using System.Collections.Generic;

/// <summary>
///     This component must be added to every entity subject to forces.
/// </summary>
public class SubjectToForces : MonoBehaviour {
    [SerializeField] public List<ForceSpec> appliedForces = new List<ForceSpec>();
    [SerializeField] public float speed = 0.1f;
}