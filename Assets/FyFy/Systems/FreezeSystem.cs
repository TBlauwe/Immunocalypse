using UnityEngine;
using FYFY;

public class FreezeSystem : FSystem {
    private readonly Family _frozen = FamilyManager.getFamily(
        new AllOfComponents(typeof(Frozen)),
        new NoneOfComponents(typeof(Removed))
    );
	
    // Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _frozen)
        {
            Frozen frozen = go.GetComponent<Frozen>();
            frozen.remainingTime -= Time.deltaTime;

            if (frozen.remainingTime <= 0)
            {
                GameObjectManager.removeComponent(frozen);
            }
        }
	}
}