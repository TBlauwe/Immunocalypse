using UnityEngine;
using System;

[Serializable]
public class FactoryEntry
{
    public GameObject prefab;
    public int nb;
    public int originalNb;

    public FactoryEntry() {}

    public FactoryEntry(GameObject prefab)
    {
        if (prefab == null) throw new ArgumentNullException();
        this.prefab = prefab;
        nb = originalNb = 1;
    }
}
