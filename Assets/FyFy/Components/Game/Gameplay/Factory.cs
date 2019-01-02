using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {
    [SerializeField] public List<FactoryEntry> entries = new List<FactoryEntry>();
    [SerializeField] public bool useRandomSpawning = false;
    [SerializeField] public float rate = 0.1f;
    [HideInInspector] public float remaining = 0;
    [SerializeField] public bool paused = false;
}