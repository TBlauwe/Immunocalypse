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
            WithHealth withHealth = go.GetComponent<WithHealth>();

            if (withHealth && withHealth.deathParticles)
            {
                Object.Instantiate(withHealth.deathParticles, go.GetComponent<Collider>().bounds.center, go.transform.rotation);
            }

            GameObjectManager.unbind(go);
            Object.Destroy(go);
        }
	}
}