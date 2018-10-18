using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;

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
                int index = (int)Random.value * _defense_sites.Count;
                movable.target = _defense_sites.getAt(index);
            }
        }

        foreach (GameObject gameObject in _macrophages)
        {
            Movable movable = gameObject.GetComponent<Movable>();
            Macrophage macrophage = gameObject.GetComponent<Macrophage>();

            continue;

            if (movable.target == null && _pathogenes.Count > 0)
            {
                Triggered3D triggered = macrophage.targetDetectionRange.GetComponent<Triggered3D>();
                if (triggered != null)
                {
                    int index = (int)Random.value * triggered.Targets.Length;
                    movable.target = triggered.Targets[index];
                }
            }
        }
    }
}