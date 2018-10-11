using UnityEngine;
using FYFY;

public class DeadRemovingSystem : FSystem {
    private readonly Family _withHealthEntities = FamilyManager.getFamily(new AllOfComponents(
        typeof(WithHealth)
    ));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _withHealthEntities)
        {
            WithHealth health = go.GetComponent<WithHealth>();
            if (health.health <= 0)
            {
                // In the future, should play particles and sound
                GameObjectManager.unbind(go);
                Object.Destroy(go);
            }
        }
	}
}