using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;

public class EatingSystem : FSystem {
    private readonly Family _macrophages = FamilyManager.getFamily(
        new AllOfComponents(typeof(Eater)),
        new NoneOfComponents(typeof(Dragable))
    );

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _macrophages)
        {
            // We suppose that there is a gameobject reference in eatingRange field in the Eater component
            Eater eater = go.GetComponent<Eater>();
            Triggered3D triggered = go.GetComponent<Eater>().eatingRange.GetComponent<Triggered3D>();

            // Decrease remaining time
            eater.remaining -= Time.deltaTime;

            // If remaining is not less or equals than 0, we cannot eat
            if (eater.remaining > 0) continue;

            // Something is in our eating range
            if (triggered != null)
            {
                bool hasEaten = false;
                foreach (GameObject target in triggered.Targets)
                {
                    if (!target.activeSelf || target.GetComponent<Removed>() != null || Random.value > eater.eatingProbability) continue;
                    Eatable eatable = target.GetComponent<Eatable>();

                    // If we can eat it (layer is good)
                    if (eatable != null && target.activeSelf && (eatable.eatableLevel & eater.eatingMask) > 0)
                    {
                        // Destroy eaten object
                        GameObjectManager.addComponent<RemoveForces>(target);
                        GameObjectManager.addComponent<Removed>(target);
                        hasEaten = true;
                    }
                }

                // If the macrophage ate some thing, then a cooldown is set
                if (hasEaten) eater.remaining = eater.cooldown;
            }
        }
	}
}