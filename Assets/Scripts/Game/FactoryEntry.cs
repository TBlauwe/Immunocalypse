using UnityEngine;
using System;

[Serializable]
public class FactoryEntry
{
    public GameObject prefab;
    public int nb;
    public int originalNb;
    public int layer;

    public FactoryEntry() {}

    public FactoryEntry(GameObject prefab)
    {
        if (prefab == null) throw new ArgumentNullException();
        this.prefab = prefab;
        nb = 1;
        originalNb = 1;
    }

    public override string ToString()
    {
        return "Entry(" + originalNb + "," + nb + ")";
    }
}
