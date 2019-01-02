using FYFY;
using System;
using UnityEngine;

[Serializable]
public class Wave{
    public Factory unitsFactory;
    public int launchingTime;
    [HideInInspector] public bool launched;

    public void launch(GameObject parentGameObject)
    {   
        if (launched) return;
        Factory factory = UnityEngine.Object.Instantiate(unitsFactory, parentGameObject.transform);
        GameObjectManager.bind(factory.gameObject);
        launched = true;
    }
}
