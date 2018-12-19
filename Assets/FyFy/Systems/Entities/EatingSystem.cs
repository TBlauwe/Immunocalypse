using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;

public class EatingSystem : FSystem {
    private readonly Family _macrophages = FamilyManager.getFamily(
        new AllOfComponents(typeof(Macrophage)),
        new NoneOfComponents(typeof(Dragable))
    );

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _macrophages)
        {
            // We suppose that there is a gameobject reference in eatingRange field in the Macrophage component
            Macrophage eater = go.GetComponent<Macrophage>();
            Triggered3D triggered = go.GetComponent<Macrophage>().eatingRange.GetComponent<Triggered3D>();

            // Something is in our eating range
            if (triggered != null)
            {
                foreach (GameObject target in triggered.Targets)
                {
                    if (!target.activeSelf || target.GetComponent<Removed>() != null) continue;
                    Eatable eatable = target.GetComponent<Eatable>();

                    // If we can eat it (layer is good)
                    if (eatable != null && target.activeSelf && (eatable.eatableLevel & eater.eatingMask) > 0)
                    {
                        // Destroy eaten object
                        GameObjectManager.addComponent<RemoveForces>(target);
                        GameObjectManager.addComponent<Removed>(target);
                    }
                }
            }
        }
	}
}