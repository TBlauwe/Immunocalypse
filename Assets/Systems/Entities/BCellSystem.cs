using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;
using FYFY_plugins.TriggerManager;

public class BCellSystem : FSystem {

    private readonly Family _BCells = FamilyManager.getFamily(new AllOfComponents(typeof(BCell)));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject goBCell in _BCells)
        {
            BCell bCell = goBCell.GetComponent<BCell>();

            // If user clicks on the cell and cell is ready
            if (goBCell.GetComponent<PointerOver>() && Input.GetKeyDown(KeyCode.Mouse0) && bCell.remaining <= 0)
            {
                GameObject[] targets = goBCell.GetComponent<Triggered3D>().Targets;
                foreach (GameObject target in targets)
                {
                    if (target.layer == 9)  // Pathogene layer
                    {
                        GameObjectManager.addComponent<Freeze>(target, new { remaining = bCell.effectDuration});
                    }
                }
                bCell.remaining = bCell.delay;
            }
            else if (bCell.remaining > 0)
            {
                bCell.remaining -= Time.deltaTime;
            }
        }
	}
}