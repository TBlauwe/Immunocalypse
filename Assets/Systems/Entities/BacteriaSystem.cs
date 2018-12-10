using UnityEngine;
using FYFY;
using System.Collections.Generic;
using FYFY_plugins.TriggerManager;

public class BacteriaSystem : FSystem
{
    // All bacterias in the game
    private readonly Family bacterias = FamilyManager.getFamily(
        new AllOfComponents(typeof(Bacteria)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF),
        new NoneOfComponents(typeof(Removed))
    );

    private readonly Family _potentialLeaders = FamilyManager.getFamily(
        new NoneOfComponents(typeof(ForceManaged), typeof(Removed)), new AllOfComponents(typeof(Bacteria), typeof(Triggered3D)),
        new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );

    protected override void onProcess(int familiesUpdateCount)
    {
        // Replication

        foreach (GameObject entity in bacterias)
        {
            Bacteria bacteria = entity.GetComponent<Bacteria>();

            // Bacteria replication
            if (Random.value <= bacteria.duplicationProbability)
            {
                // Create clone
                GameObject clone = Object.Instantiate(entity.GetComponent<Origin>().sourceObject);
                clone.transform.position = entity.transform.position;

                // Bind it to FYFY
                GameObjectManager.bind(clone);
            }
        }
    }
}