using UnityEngine;
using FYFY;

public class BacteriaSystem : FSystem
{
    // All bacterias in the game
    private readonly Family bacterias = FamilyManager.getFamily(
        new AllOfComponents(typeof(Bacteria)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF),
        new NoneOfComponents(typeof(Removed))
    );

    private readonly Family prefabsHolders = FamilyManager.getFamily(
        new AllOfComponents(typeof(PrefabHolder))
    );

    protected override void onProcess(int familiesUpdateCount)
    {
        // Get prefab holder
        GameObject goHolder = prefabsHolders.First();
        if (goHolder == null)
        {
            Debug.LogError("Couldn't find PrefabHolder, cannot instanciate bacteria (skipped)");
        }
        PrefabHolder holder = goHolder.GetComponent<PrefabHolder>();

        // Replication
        foreach (GameObject entity in bacterias)
        {
            Bacteria bacteria = entity.GetComponent<Bacteria>();
            if (Random.value <= bacteria.duplicationProbability)
            {
                // Create new fresh bacteria
                GameObject clone = Object.Instantiate(holder.SimpleBacteria);
                clone.transform.position = entity.transform.position;

                // Bind it to FYFY
                GameObjectManager.bind(clone);
            }
        }
    }
}