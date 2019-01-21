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

            bool shouldRecompute = false;

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

                    continue;
                }

                if (macrophage.lastDescision.Equals(DECISIONS.CHASE)) // Last decision was chase but nothing is eatable in our range
                {
                    shouldRecompute = true;  // We need to recompute a path to the closest EndNode.
                }
            }
            
            if (macrophage.lastDescision.Equals(DECISIONS.CHASE) || shouldRecompute) // No pathogene to hunt in the area
            {
                // Recompute closest Waypoint
                GameObject _target = PathSystem.GetClosestWaypoint(go, _Waypoints);
                PathFollower follower = go.GetComponent<PathFollower>();

                // Update destination
                follower.destination = PathSystem.ComputeDestination(_target.transform.position, _EndWaypoints);
                move.target = _target.transform.position;

                // Update decision
                macrophage.lastDescision = DECISIONS.FOLLOW_PATH;
            }
            
        }
    }
}