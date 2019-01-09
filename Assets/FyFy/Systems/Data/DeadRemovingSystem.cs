using UnityEngine;
using FYFY;

public class DeadRemovingSystem : FSystem {

    private readonly Family _toRemove = FamilyManager.getFamily(new AllOfComponents(
        typeof(Removed)
    ));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _toRemove)
        {
            GameObjectManager.unbind(go);
            Object.Destroy(go);
        }
	}
}