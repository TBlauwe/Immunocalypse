﻿using UnityEngine;
using FYFY;

public class FreezeSystem : FSystem {
    private readonly Family _frozen = FamilyManager.getFamily(
        new AllOfComponents(typeof(Frozen), typeof(Rigidbody)),
        new NoneOfComponents(typeof(Removed))
    );
	
    // Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _frozen)
        {
            // Update cooldown
            Frozen frozen = go.GetComponent<Frozen>();
            frozen.remainingTime -= Time.deltaTime;

            // Set Velocity to 0
            go.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (frozen.remainingTime <= 0)
            {
                GameObjectManager.removeComponent(frozen);
            }
        }
	}
}