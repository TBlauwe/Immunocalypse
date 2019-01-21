using UnityEngine;

[RequireComponent(typeof(MoveToward))]
public class PathFollower : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public Node destination;
    public Node nextWaypoint;
    public Node previousWaypoint;
}