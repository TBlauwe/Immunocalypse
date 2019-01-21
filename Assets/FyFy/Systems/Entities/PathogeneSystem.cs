using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;
using System.Collections.Generic;

public class PathogeneSystem : FSystem {
    // COMMON STUFF
    // All active pathogenes in the game
    private readonly Family _Active = FamilyManager.getFamily(
        new AllOfComponents(typeof(Pathogene)),
        new AnyOfLayers(9),  // pathogene layer
        new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF),
        new NoneOfComponents(typeof(Removed), typeof(Frozen))
    );

    // Where to find the original prefab
    private readonly Family _YellowPages = FamilyManager.getFamily(
        new AllOfComponents(typeof(YellowPageComponent))
    );

    private readonly Family _Waypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(Node))
    );

    // Targets
    private readonly Family _Cells = FamilyManager.getFamily(
        new AllOfComponents(typeof(Cell)),
        new NoneOfComponents(typeof(Removed))
    );

    // SPECIALIZED STUFF
    private readonly Family _Bacterias = FamilyManager.getFamily(
        new AllOfComponents(typeof(Bacteria)),
        new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF),
        new NoneOfComponents(typeof(Removed), typeof(Frozen))
    );

    private readonly Family _Virus = FamilyManager.getFamily(
        new AllOfComponents(typeof(Virus)),
        new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF),
        new NoneOfComponents(typeof(Removed), typeof(Frozen))
    );

    // ATTRIBUTES
    private YellowPageComponent holder;

    // OTHER
    public static readonly int MAX_NB_OF_BACTERIAS = 100;
    public static readonly float SPEED_SHIFT = 0.1f;

    public PathogeneSystem()
    {
        // Handle specialized stuff
        // Bacterias
        foreach (GameObject go in _Bacterias)
        {
            InitBacteria(go);
        }
        _Bacterias.addEntryCallback(InitBacteria);

        // Virus
        foreach (GameObject go in _Virus)
        {
            RandomizeSpeed(go);
        }
        _Virus.addEntryCallback(RandomizeSpeed);
    }

    protected override void onProcess(int familiesUpdateCount)
    {
        // Get prefab holder if it is required
        if (holder == null)
        {
            FindYellowPages();
        }

        // Process main stuff
        foreach (GameObject go in _Active)
        {
            MakeDecision(go);
        }

        // Handle specific tasks
        // Bacterias
        foreach (GameObject go in _Bacterias)
        {
            Replicate(go.GetComponent<Bacteria>());
        }
    }

    private void Replicate(Bacteria bacteria)
    {
        // Replication
        if (bacteria.replicationCooldown <= 0 && _Active.Count < MAX_NB_OF_BACTERIAS)
        {
            // Create new fresh bacteria
            Origin origin = bacteria.gameObject.GetComponent<Origin>();
            GameObject clone = Object.Instantiate(YellowPageUtils.GetSourceObject(holder, origin.sourceObjectKey));
            clone.transform.position = bacteria.transform.position;

            // Bind it to FYFY
            GameObjectManager.bind(clone);

            // Reset cooldown
            bacteria.replicationCooldown = bacteria.replicationDelay;
        }
        else
        {
            bacteria.replicationCooldown -= Time.deltaTime;
        }
    }

    private void MakeDecision(GameObject go)
    {
        Pathogene pathogene = go.GetComponent<Pathogene>();
        PathFollower follower = go.GetComponent<PathFollower>();
        MoveToward move = go.GetComponent<MoveToward>();

        if (pathogene.target == null) // No target, maybe already dead or pathogene just spawned
        {
            pathogene.target = FindNextTarget(go.transform.position);
            if (pathogene.target == null) return;

            Node next = GetClosestWaypointTo(go.transform.position).GetComponent<Node>();
            Node dest = GetClosestWaypointTo(pathogene.target.transform.position).GetComponent<Node>();

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
            move.target = pathogene.target.transform.position;
        }

        // Pathogenes are opportunists : if a cell is closer than their target, they will go kill it
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

    private void FindYellowPages()
    {
        // Get prefab holder
        GameObject goHolder = _YellowPages.First();
        if (goHolder == null)
        {
            Debug.LogError("Couldn't find PrefabHolder, cannot instanciate pathogene");
        }
        holder = goHolder.GetComponent<YellowPageComponent>();
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

    private void InitBacteria(GameObject go)
    {
        Bacteria b = go.GetComponent<Bacteria>();
        b.replicationCooldown = Random.Range(0, b.replicationDelay);
    }

    private void RandomizeSpeed(GameObject go)
    {
        MoveToward mv = go.GetComponent<MoveToward>();
        mv.speed = Random.Range(mv.speed - SPEED_SHIFT, mv.speed + SPEED_SHIFT);
    }
}