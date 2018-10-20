using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;
using System.Collections.Generic;

public class TargetComputerSystem : FSystem {
    private readonly Family _pathogenes = FamilyManager.getFamily(new AnyOfTags("pathogene"), new AnyOfComponents(typeof(Movable)));
    private readonly Family _macrophages = FamilyManager.getFamily(new AllOfComponents(typeof(Macrophage)));

    private readonly Family _defense_sites = FamilyManager.getFamily(new AnyOfTags("defense_site"));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject gameObject in _pathogenes)
        {
            Movable movable = gameObject.GetComponent<Movable>();
            if (_defense_sites.Count > 0 && movable.target == null)
            {
                int index = (int) (Random.value * _defense_sites.Count);
                movable.target = _defense_sites.getAt(index);
            }
        }

        /*foreach (GameObject gameObject in _macrophages)
        {
            Movable movable = gameObject.GetComponent<Movable>();
            Macrophage macrophage = gameObject.GetComponent<Macrophage>();
            Eater eater = gameObject.GetComponent<Eater>();

            Triggered3D detectedStuff = macrophage.targetDetectionRange.GetComponent<Triggered3D>();

            if (movable.target == null && detectedStuff != null && detectedStuff.Targets.Length > 0)
            {
                List<GameObject> targets = new List<GameObject>();
                foreach (GameObject target in detectedStuff.Targets)
                {
                    Eatable eatable = target.GetComponent<Eatable>();
                    if (eatable != null && (eatable.eatableLevel & eater.eatMask) > 0)
                    {
                        targets.Add(target);
                    }
                }

                if (targets.Count > 0)
                {
                    int index = (int)(Random.value * targets.Count);
                    movable.target = targets[index];
                }
            }
        }*/
    }
}