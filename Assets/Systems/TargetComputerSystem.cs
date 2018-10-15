using UnityEngine;
using FYFY;

public class TargetComputerSystem : FSystem {
    private readonly Family _pathogenes = FamilyManager.getFamily(new AnyOfTags("pathogene"), new AnyOfComponents(typeof(Movable)));
    private readonly Family _immunos = FamilyManager.getFamily(new AnyOfTags("immuno"), new AnyOfComponents(typeof(Movable)));
    private readonly Family _defense_sites = FamilyManager.getFamily(new AnyOfTags("defense_site"));

    private readonly Family _movables = FamilyManager.getFamily(new AllOfComponents(typeof(Movable)));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject gameObject in _pathogenes)
        {
            Movable movable = gameObject.GetComponent<Movable>();
            {
                if (_defense_sites.Count > 0)
                {
                    movable.target = _defense_sites.First();
                }
            }
        }

        foreach (GameObject gameObject in _immunos)
        {
            Movable movable = gameObject.GetComponent<Movable>();
            if (movable.target == null)
            {
                if (_pathogenes.Count > 0) movable.target = _pathogenes.First();
            }
        }
    }
}