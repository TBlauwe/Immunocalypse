using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;

public class DamagingSystem : FSystem {

    private Family damagersFamily = FamilyManager.getFamily(new AllOfComponents(typeof(Damager), typeof(Triggered3D)));
    private Family destroyablesFamily = FamilyManager.getFamily(new AllOfComponents(typeof(Destroyable)));
    private int damagingsPerSecond = 1;
    private float lastTimeDamaged = 0;

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){

    }

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        lastTimeDamaged += Time.deltaTime;
        if(lastTimeDamaged>=1/damagingsPerSecond)
        {
            lastTimeDamaged = 0;
            foreach (GameObject attacker in damagersFamily)
            {
                Triggered3D trigger3D = attacker.GetComponent<Triggered3D>();
                Damager damager = attacker.GetComponent<Damager>();
                foreach (GameObject target in trigger3D.Targets)
                {
                    if (target.Equals(attacker)) continue;
                    if (destroyablesFamily.contains(target.GetInstanceID()))
                    {
                        Destroyable destroyable = target.GetComponent<Destroyable>();
                        if ((damager.damagablesLayerMask & destroyable.layerMask) > 0)
                        {
                            WithHealth health = target.GetComponent<WithHealth>();
                            if (damager.attackPoints > destroyable.defensePoints)
                            {
                                health.health -= damager.attackPoints - destroyable.defensePoints;
                            }
                        }
                    }
                }
            }
        }
	}
}