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

    public PathSystem() : base()
    {
        foreach (GameObject nodeGO in _waypoints)
        {
            _graph.nodes.Add(nodeGO.GetComponent<Node>());
            nodeGO.GetComponent<MeshRenderer>().enabled = false;
        }
        // Maybe add a listener on the family ?
    }


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
        foreach (GameObject waypointGO in _triggeredWaypoints)
        {
            GameObject[] targets = waypointGO.GetComponent<Triggered3D>().Targets;
            foreach (GameObject targetGO in targets)
            {
                PathFollower follower = targetGO.GetComponent<PathFollower>();
                Node current = waypointGO.GetComponent<Node>();
                if (follower != null && (follower.previousWaypoint == null || !follower.previousWaypoint.Equals(current)))
                {
                    ComputeNextWaypoint(targetGO, follower, current);
                    follower.previousWaypoint = current;
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
            //Vector3 targetDir = follower.nextWaypoint.transform.position - go.transform.position;

            //Vector3 newDir = Vector3.RotateTowards(go.transform.forward, targetDir, 2 * Mathf.PI, 0.0f);
            //Debug.DrawRay(go.transform.position, newDir, Color.blue);

            //go.transform.rotation = Quaternion.LookRotation(newDir);
            //go.transform.rotation = Quaternion.LookRotation(newDir);

            // Remove step from path
            path.nodes.RemoveAt(0);
            //followersPath.Add(go, path);
        }
    }

    private void ComputePathTo(GameObject go, Node start, Node end)
    {
        Path path = _graph.GetShortestPath(start, end);
        path.nodes.RemoveAt(0);
        followersPath.Add(go, path);
    }
}