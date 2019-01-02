using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;
using FYFY_plugins.TriggerManager;

public class MacrophageSystem : FSystem {

    public enum DECISIONS
    {
        CHASE,
        FOLLOW_PATH
    }

    private readonly Family _InVesselPointerOver = FamilyManager.getFamily(
        new AllOfComponents(typeof(Macrophage), typeof(PointerOver)),
        new NoneOfLayers(11) // Immuno layer
    );

    private readonly Family _Waypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(Node))
    );

    private readonly Family _EndWaypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(EndNode))
    );

    private readonly Family _ToRepop = FamilyManager.getFamily(
        new AllOfComponents(typeof(Macrophage)),
        new NoneOfComponents(typeof(PathFollower)),
        new AnyOfLayers(11)
    );

    private readonly Family _StartTrigger = FamilyManager.getFamily(
        new AllOfComponents(typeof(StartLoopTrigger))
    );

    private readonly Family _Active = FamilyManager.getFamily(
        new AllOfComponents(typeof(Macrophage), typeof(PathFollower)),
        new AnyOfLayers(11) // Immuno layer
    );

    private readonly Family _YellowPages = FamilyManager.getFamily(
        new AllOfComponents(typeof(YellowPageComponent))
    );

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
        ProcessInVesselPointerOver();
        ProcessUseless();
        MakeDecision();
    }

    private void ProcessInVesselPointerOver()
    {
        foreach (GameObject go in _InVesselPointerOver)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObjectManager.setGameObjectLayer(go, 11); // Now macrophage is considered as an immunity cell
                MoveToward move = go.GetComponent<MoveToward>();
                GameObject target = GetClosestWaypoint(go);

                GameObjectManager.addComponent(go, typeof(PathFollower), new {
                    destination = ComputeDestination(target.transform.position)
                });  // Compute destination (the closest one from start waypoint)

                move.target = target.transform.position; // Move cell to closest waypoint
            }
        }
    }

    private GameObject GetClosestWaypoint(GameObject src)
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject nodeGO in _Waypoints)
        {
            float distance = Vector3.Distance(src.transform.position, nodeGO.transform.position);
            if (distance < minDistance)
            {
                closest = nodeGO;
                minDistance = distance;
            }
        }

        return closest;
    }

    private Node ComputeDestination(Vector3 start)
    {
        Node selected = null;
        float currentDistance = float.MaxValue;
        foreach (GameObject go in _EndWaypoints)
        {
            float distance = Vector3.Distance(start, go.transform.position);
            if (distance < currentDistance)
            {
                selected = go.GetComponent<Node>();
                currentDistance = distance;
            }
        }
        return selected;
    }

    private void ProcessUseless()
    {
        StartLoopTrigger start = _StartTrigger.First().GetComponent<StartLoopTrigger>();
        foreach (GameObject go in _ToRepop)
        {
            GameObjectManager.addComponent<Removed>(go);
            YellowPageComponent yp = _YellowPages.First().GetComponent<YellowPageComponent>();
            start.deckPool.Add(YellowPageUtils.GetSourceObject(yp, "Macrophage"));
        }
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
                GameObject target = GetClosestWaypoint(go);

                // Go to closest waypoint and update destination
                move.target = target.transform.position;
                follower.destination = ComputeDestination(target.transform.position);
            }
        }
    }
}