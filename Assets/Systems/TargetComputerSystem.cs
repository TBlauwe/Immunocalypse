using UnityEngine;
using FYFY;

public class TargetComputerSystem : FSystem {
    private readonly Family _pathogenes = FamilyManager.getFamily(new AnyOfTags("pathogene"));
    private readonly Family _immunos = FamilyManager.getFamily(new AnyOfTags("immuno"));
    private readonly Family _defense_sites = FamilyManager.getFamily(new AnyOfTags("defense_site"));

    private readonly Family _movables = FamilyManager.getFamily(new AllOfComponents(typeof(Movable)));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _movables)
        {
            Movable movable = go.GetComponent<Movable>();
            if (movable.target == null)
            {
                string tag = go.tag;
                Debug.Log(tag);
                switch(tag)
                {
                    case "pathogene":
                        if (_defense_sites.Count > 0) movable.target = _defense_sites.First();
                        break;
                    case "immuno":
                        if (_pathogenes.Count > 0) movable.target = _pathogenes.First();
                        break;
                }
            }
        }
	}
}