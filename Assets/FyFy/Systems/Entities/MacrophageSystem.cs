using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;

public class MacrophageSystem : FSystem {
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
    }

    private void ProcessInVesselPointerOver()
    {
        foreach (GameObject go in _InVesselPointerOver)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObjectManager.setGameObjectLayer(go, 11); // Now macrophage is considered as an immunity cell
                MoveToward move = go.GetComponent<MoveToward>();
                GameObject target = GetClosestwaypoint(go);

                GameObjectManager.addComponent(go, typeof(PathFollower), new {
                    destination = ComputeDestination(target.transform.position)
                });  // Compute destination (the closest one from start waypoint)

                move.target = target.transform.position; // Move cell to closest waypoint
            }
        }
    }

    private GameObject GetClosestwaypoint(GameObject src)
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
            GameObjectManager.setGameObjectLayer(go, 0);
            GameObjectManager.setGameObjectState(go, false);
            GameObjectManager.unbind(go);
            start.deckPool.Add(go);
        }
    }
}