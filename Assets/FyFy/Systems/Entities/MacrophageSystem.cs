using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;

public class MacrophageSystem : FSystem {

    public enum DECISIONS
    {
        CHASE,
        FOLLOW_PATH
    }

    private readonly Family _Waypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(Node))
    );

    private readonly Family _EndWaypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(EndNode))
    );

    private readonly Family _Active = FamilyManager.getFamily(
        new AllOfComponents(typeof(Macrophage), typeof(PathFollower)),
        new AnyOfLayers(11) // Immuno layer
    );

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        MakeDecision();
    }

    private void MakeDecision()
    {
        foreach (GameObject go in _Active)
        {
            Macrophage macrophage = go.GetComponent<Macrophage>();
            MoveToward move = go.GetComponent<MoveToward>();
            Triggered3D triggered = go.GetComponent<Triggered3D>();

            // Default is FOLLOW_PATH
            macrophage.lastDescision = DECISIONS.FOLLOW_PATH;

            // Is the a pathogene nearby ?
            if (triggered != null)
            {
                Eater eater = go.GetComponent<Eater>();
                float minDistance = float.MaxValue;
                GameObject targetedPosition = null;

                // Compute closest eatable thing
                foreach (GameObject target in triggered.Targets)
                {
                    Eatable eatable = target.GetComponent<Eatable>();
                    if (eatable != null && (eatable.eatableMask & eater.eatingMask) > 0)
                    {
                        float distance = Vector3.Distance(go.transform.position, target.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            targetedPosition = target;
                        }
                    }
                }

                if (targetedPosition != null) // Found something to eat
                {
                    // Move to it
                    move.target = targetedPosition.transform.position;

                    // Update last decision made
                    macrophage.lastDescision = DECISIONS.CHASE;
                }
            }
            else if (macrophage.lastDescision.Equals(DECISIONS.FOLLOW_PATH)) // No pathogene to hunt in the area
            {
                // Recompute destination
                PathFollower follower = go.GetComponent<PathFollower>();
                GameObject target = PathSystem.GetClosestWaypoint(go, _Waypoints);

                // Go to closest waypoint and update destination
                move.target = target.transform.position;
                follower.destination = PathSystem.ComputeDestination(target.transform.position, _EndWaypoints);
            }
        }
    }
}