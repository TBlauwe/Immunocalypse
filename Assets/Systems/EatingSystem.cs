using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;

public class EatingSystem : FSystem {
    private readonly Family _eaters = FamilyManager.getFamily(
        new AllOfComponents(typeof(Eater), typeof(Triggered3D)),
        new NoneOfComponents(typeof(Macrophage))
    );
    private readonly Family _macrophages = FamilyManager.getFamily(
        new AllOfComponents(typeof(Macrophage)),
        new NoneOfComponents(typeof(Dragable))
    );

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

        // Non-macrophages eaters (for now, nobody ?)
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

            // If eating limit reaches 0, the game object must die
            if (eater.eatingLimitBeforeDeath == 0)
            {
                WithHealth withHealth = go.GetComponent<WithHealth>();
                withHealth.health = 0;
            }
        }

        foreach (GameObject go in _macrophages)
        {
            // We suppose that the is a gameobject reference in eatingRange field in the Macrophage component
            Macrophage eater = go.GetComponent<Macrophage>();
            Triggered3D triggered = go.GetComponent<Macrophage>().eatingRange.GetComponent<Triggered3D>();

            // Something is in our eating range
            if (triggered != null)
            {
                foreach (GameObject target in triggered.Targets)
                {
                    Eatable eatable = target.GetComponent<Eatable>();

                    // If we can eat it (layer is good) and cooldown is ok
                    if (eatable != null && (eatable.eatableLevel & eater.eatingMask) > 0)
                    {
                        // Destroy eaten object
                        try
                        {
                            GameObjectManager.unbind(target);
                            Object.Destroy(target);
                        }
                        catch (UnknownGameObjectException) { continue; }  // If another macrophage has eaten our target while computing                     
                        catch (MissingReferenceException) { continue; }  // If another macrophage has eaten our target while computing                     
                    }
                }
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