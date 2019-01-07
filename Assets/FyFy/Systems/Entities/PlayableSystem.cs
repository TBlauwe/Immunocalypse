using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;

public class PlayableSystem : FSystem {
    private readonly Family _InVesselPointerOver = FamilyManager.getFamily(
        new AllOfComponents(typeof(Playable), typeof(PointerOver)),
        new NoneOfLayers(11) // Immuno layer
    );

    private readonly Family _EndWaypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(EndNode))
    );

    private readonly Family _Waypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(Node))
    );

    // Use to process your families.
    protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _InVesselPointerOver)
        {
            ProcessInVesselPointerOver();
        }
	}

    private void ProcessInVesselPointerOver()
    {
        foreach (GameObject go in _InVesselPointerOver)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObjectManager.setGameObjectLayer(go, 11); // Now macrophage is considered as an immunity cell
                MoveToward move = go.GetComponent<MoveToward>();
                Playable playable = go.GetComponent<Playable>();
                if (playable)
                {
                    //Global.data.trackedEntities.
                }
                GameObject target = GetClosestWaypoint(go);

                GameObjectManager.addComponent(go, typeof(PathFollower), new
                {
                    destination = ComputeDestination(target.transform.position)
                });  // Compute destination (the closest one from start waypoint)

                move.target = target.transform.position; // Move cell to closest waypoint
            }
        }
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
}