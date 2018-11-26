using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ForceManager : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public List<GameObject> children = new List<GameObject>();
}