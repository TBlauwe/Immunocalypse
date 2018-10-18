using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;

public class EatingSystem : FSystem {
    private readonly Family _eaters = FamilyManager.getFamily(new AllOfComponents(
        typeof(Eater), typeof(Triggered3D)
    ));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _eaters)
        {
            Eater eater = go.GetComponent<Eater>();
            Triggered3D triggered = go.GetComponent<Triggered3D>();

            // Decrease nextMeal
            if (eater.cooldown > 0) {
                eater.cooldown -= Time.deltaTime;
            }

            // Something is in our eating range
            if (triggered != null)
            {
                Eat(eater, triggered);
            }

            if (eater.eatingLimitBeforeDeath <= 0)
            {
                WithHealth withHealth = go.GetComponent<WithHealth>();
                withHealth.health = 0;
            }
        }
	}

    private void Eat(Eater eater, Triggered3D triggered)
    {
        int i = 0;
        bool haveEaten = false;
        while (eater.eatingLimitBeforeDeath > 0 && !haveEaten && i < triggered.Targets.Length)
        {
            Eatable eatable = triggered.Targets[i].GetComponent<Eatable>();

            // If we can eat it (layer is good) and cooldown is ok
            if (eatable != null && (eatable.eatableLevel & eater.eatMask) > 0 && eater.cooldown <= 0)
            {
                // Need some rest before eating again
                eater.cooldown = eater.eatDelta;

                // Destroy eaten object
                GameObjectManager.unbind(triggered.Targets[i]);
                Object.Destroy(triggered.Targets[i]);

                // We have eaten
                haveEaten = true;
                eater.eatingLimitBeforeDeath -= 1;
            }
            ++i;
        }
    }
}