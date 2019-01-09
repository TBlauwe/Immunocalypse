using UnityEngine;
using FYFY;
using FYFY_plugins.CollisionManager;

public class DamagingSystem : FSystem {
    private readonly Family _Damagers = FamilyManager.getFamily(
        new AllOfComponents(typeof(Damager), typeof(InCollision3D)),
        new NoneOfComponents(typeof(Removed))
    );
	
    // Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _Damagers)
        {
            // Decrease cooldown
            Damager damager = go.GetComponent<Damager>();
            damager.cooldown -= Time.deltaTime;

            // Can the damager attack ?
            if (damager.cooldown > 0) continue;

            // It does, so check if it can hit something
            InCollision3D collider = go.GetComponent<InCollision3D>();
            if (collider.Targets.Length == 0) continue;

            // Something is in collision, can we damage it ?
            GameObject target = collider.Targets[0];
            WithHealth targetWH = target.GetComponent<WithHealth>();
            int i = 1;

            while (i < collider.Targets.Length && targetWH == null)
            {
                target = collider.Targets[i];
                targetWH = target.GetComponent<WithHealth>();
                ++i;
            }
            if (i == collider.Targets.Length) continue;

            // We can ! Hit and reset cooldown
            targetWH.health -= damager.damagesPerSec;
            damager.cooldown = 1;
        }
	}
}