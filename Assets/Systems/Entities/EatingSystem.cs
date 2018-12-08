﻿using UnityEngine;
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
                        if (target.GetComponent<Bacteria>() != null)
                        {
                            DestroyBacteria(target);
                        }
                        //GameObjectManager.unbind(target);
                        //Object.Destroy(target);
                        GameObjectManager.addComponent<Removed>(target);
                    }
                }
            }
        }
	}

    private void DestroyBacteria(GameObject bacteria)
    {
        ForceManager manager = bacteria.GetComponent<ForceManager>();
        if (manager != null)
        {
            foreach (GameObject child in manager.children)
            {
                if (child == null) continue;
                GameObjectManager.removeComponent<SpringJoint>(child);
                GameObjectManager.removeComponent<ForceManaged>(child);

                GameObjectManager.addComponent<ForceCreator>(
                    child,
                    new { forceLayerMask = bacteria.GetComponent<ForceCreator>().forceLayerMask }
                );

                SubjectToForces subjectToForces = bacteria.GetComponent<SubjectToForces>();
                GameObjectManager.addComponent<SubjectToForces>(
                        child,
                        new { appliedForces = subjectToForces.appliedForces, speed = subjectToForces.speed }
                    );
            }
        }

        ForceManaged managed = bacteria.GetComponent<ForceManaged>();
        if (managed != null && managed.parent != null)
        {
            ForceManager parentManager = managed.parent.GetComponent<ForceManager>();
            if (parentManager != null)
            {
                parentManager.children.Remove(bacteria);
            }
        }
    }
}