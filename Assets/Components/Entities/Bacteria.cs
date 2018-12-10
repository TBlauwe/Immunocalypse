using FYFY_plugins.TriggerManager;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Origin))]
public class Bacteria : MonoBehaviour {
    [Range(0, 1)] public float duplicationProbability;
    // public GameObject forceLeader;
}