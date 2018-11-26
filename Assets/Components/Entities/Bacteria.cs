using FYFY_plugins.TriggerManager;
using System.Collections.Generic;
using UnityEngine;

public class Bacteria : MonoBehaviour {
    [Range(0, 1)] public float duplicationProbability;
    public GameObject forceLeader;
    // public List<ForceCreator> forces = new List<ForceCreator>();
}