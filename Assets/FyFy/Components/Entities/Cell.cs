using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WithHealth))]
public class Cell : MonoBehaviour {
    public CellState state;
    public List<FactoryEntry> infections = new List<FactoryEntry>();
}