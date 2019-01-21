using UnityEngine;
using FYFY;
using FYFY_plugins.CollisionManager;

public class EatingSystem : FSystem {
    private readonly Family _Eaters = FamilyManager.getFamily(
        new AllOfComponents(typeof(Eater), typeof(InCollision3D)),
        new NoneOfComponents(typeof(Removed))
    );

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _Eaters)
        {
            InCollision3D collision = go.GetComponent<InCollision3D>();
            Eater eater = go.GetComponent<Eater>();

            foreach (GameObject target in collision.Targets)
            {
                Eatable eatable = target.GetComponent<Eatable>();
                if (eatable != null && (eater.eatingMask & eatable.eatableMask) > 0 && Random.value <= eater.eatingProbability)
                {
                    GameObjectManager.addComponent<Removed>(target);
                }
            }
        }
	}
}