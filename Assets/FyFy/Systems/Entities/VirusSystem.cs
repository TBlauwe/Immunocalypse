using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;
using System.Collections.Generic;

public class VirusSystem : FSystem {
    // All active virus
    private readonly Family _Active = FamilyManager.getFamily(
        new AllOfComponents(typeof(Virus)),
        new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF),
        new NoneOfComponents(typeof(Removed), typeof(Frozen))
    );

    private readonly Family _Waypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(Node))
    );

    // Targets
    private readonly Family _Cells = FamilyManager.getFamily(
        new AllOfComponents(typeof(Cell)),
        new NoneOfComponents(typeof(Removed))
    );
    

    protected override void onProcess(int familiesUpdateCount)
    {
        foreach (GameObject go in _Active)
        {
            MakeDecision(go);
        }
    }

    private void MakeDecision(GameObject go)
    {
        Virus virus = go.GetComponent<Virus>();
        PathFollower follower = go.GetComponent<PathFollower>();
        MoveToward move = go.GetComponent<MoveToward>();

        if (virus.target == null) // No target, maybe already dead or virus just spawned
        {
            virus.target = FindNextTarget(go.transform.position);
            if (virus.target == null) return;

            Node next = GetClosestWaypointTo(go.transform.position).GetComponent<Node>();
            Node dest = GetClosestWaypointTo(virus.target.transform.position).GetComponent<Node>();

            if (follower == null) // We could have reach the final waypoint, PathFollower component may have been removed
            {
                GameObjectManager.addComponent<PathFollower>(go, new
                {
                    nextWaypoint = next,
                    destination = dest
                });
            }
            else
            {
                follower.nextWaypoint = next;
                follower.destination = dest;
            }
            move.target = next.transform.position;
        }
        else if (follower == null) // Final waypoint has been reached, but target still exists
        {
            move.target = virus.target.transform.position;
        }

        // Bacterias are opportunists : if a cell is closer than their target, they will go kill it
        Triggered3D triggered = go.GetComponent<Triggered3D>();
        if (triggered != null) // Cell in range ?
        {
            foreach (GameObject target in triggered.Targets)
            {
                if (target.GetComponent<Cell>() != null && target.GetComponent<Removed>() == null)
                {
                    move.target = target.transform.position;
                    return;
                }
            }
        }
    }

    private GameObject FindNextTarget(Vector3 src)
    {
        return GetClosestGameObjectTo(src, _Cells);
    }

    private GameObject GetClosestWaypointTo(Vector3 src)
    {
        return GetClosestGameObjectTo(src, _Waypoints);
    }

    private GameObject GetClosestGameObjectTo(Vector3 src, IEnumerable<GameObject> objects)
    {
        float minDistance = float.MaxValue;
        GameObject target = null;

        foreach (GameObject go in objects)
        {
            float distance = Vector3.Distance(src, go.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = go;
            }
        }
        return target;
    }
}