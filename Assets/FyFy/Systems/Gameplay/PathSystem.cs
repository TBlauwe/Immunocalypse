using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;
using System.Collections.Generic;
using WaypointSystem;

public class PathSystem : FSystem {

    private readonly Family _triggeredWaypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(Node), typeof(Triggered3D))
    );

    private readonly Family _waypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(Node))
    );

    private readonly Graph _graph = new Graph();

    private readonly Dictionary<GameObject, Path> followersPath = new Dictionary<GameObject, Path>();

    public static Node ComputeDestination(Vector3 start, IEnumerable<GameObject> endNodes)
    {
        Node selected = null;
        float currentDistance = float.MaxValue;
        foreach (GameObject go in endNodes)
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

    public static GameObject GetClosestWaypoint(GameObject src, IEnumerable<GameObject> nodes)
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject nodeGO in nodes)
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

    public PathSystem() : base()
    {
        foreach (GameObject nodeGO in _waypoints)
        {
            Node node = nodeGO.GetComponent<Node>();
            _graph.nodes.Add(node);
            nodeGO.GetComponent<MeshRenderer>().enabled = false;
            // Debug.Log(string.Format("Current Node : {0}, Neighbours: {1}", node, node.connections.Count));
        }
        // Maybe add a listener on the family ?
    }

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject waypointGO in _triggeredWaypoints)
        {
            GameObject[] targets = waypointGO.GetComponent<Triggered3D>().Targets;
            foreach (GameObject targetGO in targets)
            {
                PathFollower follower = targetGO.GetComponent<PathFollower>();
                Node current = waypointGO.GetComponent<Node>();
                if (follower != null)
                {
                    if (follower.previousWaypoint == null || !follower.previousWaypoint.Equals(current))
                    {
                        ComputeNextWaypoint(targetGO, follower, current);
                        follower.previousWaypoint = current;
                    }
                    MoveToward move = targetGO.GetComponent<MoveToward>();
                    move.target = follower.nextWaypoint.gameObject.transform.position;
                }
            }
        }
	}

    private void ComputeNextWaypoint(GameObject go, PathFollower follower, Node node)
    {
        if (!followersPath.ContainsKey(go))
        {
            ComputePathTo(go, node, follower.destination);
        }
        Path path;
        followersPath.TryGetValue(go, out path);

        if (path.nodes.Count == 0)
        {
            // No more destination
            GameObjectManager.removeComponent(follower);
            followersPath.Remove(go);
        }
        else
        {
            // Set next waypoint and face it
            follower.nextWaypoint = path.nodes[0];

            // Remove step from path
            path.nodes.RemoveAt(0);
        }
    }

    private void ComputePathTo(GameObject go, Node start, Node end)
    {
        Path path = _graph.GetShortestPath(start, end);
        path.nodes.RemoveAt(0);
        followersPath.Add(go, path);
    }
}