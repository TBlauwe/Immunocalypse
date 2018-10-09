﻿using UnityEngine;
using FYFY;

public class NewMovingSystem : FSystem {
    private Family family = FamilyManager.getFamily(new AllOfComponents(typeof(NewMovable)));

    protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){

	}

	// Use to process your families.
    // Loop throught the game objects inside the family
    // For each moving object, we direct it to its target (if it has one)
    // We compute the forces applied by the nearby objects that are in conflict (we use the layer mask for this one)
	protected override void onProcess(int familiesUpdateCount) {
        // Loop throught the game objects inside the family
        foreach (GameObject objectA in family)
        {
            
            NewMovable movableA = objectA.GetComponent<NewMovable>();
            if (movableA.is_moving)
            {
                // For each moving object, we direct it to its target (if it has one)
                Vector3 newVelocity = new Vector3();
                if (movableA.target != null && (objectA.transform.position - movableA.target).magnitude > movableA.safetyDistance)
                {
                    newVelocity += (movableA.target - objectA.transform.position).normalized;
                }
                // We compute the forces applied by the nearby objects that are in conflict (we use the layer mask for this one)
                foreach (GameObject objectB in family)
                {
                    //We make sure to avoid the same object
                    if (objectA.Equals(objectB)) continue;

                    NewMovable movableB = objectB.GetComponent<NewMovable>();

                    //We apply an AND operator to check if the objects are on a same layer
                    int and = movableA.layer_mask & movableB.layer_mask;

                    Vector3 r = (objectA.transform.position - objectB.transform.position);

                    //We retrieve the distance between them
                    float distance = r.magnitude;

                    // we should apply a force if the two objects occupy a same layer & the distance between them is inside the range
                    if (and > 0 && distance < movableA.range)
                    {
                        Vector3 u = r.normalized;
                        float a = 2.0f;
                        float b = 0.0f;
                        float n = 2f;
                        float m = 2f;

                        //We compute the force to apply
                        float U = a / Mathf.Pow(distance, n) - b / Mathf.Pow(distance, m);

                        newVelocity += u * U;
                    }
                }
                //We normalize the velocity
                newVelocity.Normalize();
                objectA.GetComponent<Rigidbody>().velocity = newVelocity * movableA.velocity;
            }
        }
    }
}