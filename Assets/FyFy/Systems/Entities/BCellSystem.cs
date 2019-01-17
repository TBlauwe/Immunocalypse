using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;
using FYFY_plugins.TriggerManager;

public class BCellSystem : FSystem {
    private readonly Family _BCells = FamilyManager.getFamily(
        new AllOfComponents(typeof(BCell)),
        new NoneOfComponents(typeof(Removed)),
        new AnyOfLayers(11)
    );

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _BCells)
        {
            BCell bCell = go.GetComponent<BCell>();
            bCell.cooldown -= Time.deltaTime;

            if (go.GetComponent<PointerOver>() != null && Input.GetKeyDown(KeyCode.Mouse0) && bCell.cooldown <= 0)
            {
                Freeze(bCell);
                Object.Instantiate(bCell.onClickParticleEffect, go.GetComponent<Collider>().bounds.center, go.transform.rotation);
            }
        }
	}

    private void Freeze(BCell src)
    {
        Triggered3D trigerred = src.gameObject.GetComponent<Triggered3D>();

        if (trigerred != null)
        {
            foreach (GameObject target in trigerred.Targets)
            {
                Eatable eatable = target.GetComponent<Eatable>();
                if (eatable != null && (eatable.eatableMask & src.freezeMask) > 0 && target.GetComponent<Removed>() == null)
                {
                    GameObjectManager.addComponent<Frozen>(target, new { remainingTime = src.freezeTime });
                }
            }

            if (trigerred.Targets.Length > 0)
            {
                src.cooldown = src.delay;
            }
        }
    }
}