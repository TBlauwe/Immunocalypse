using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;

public class PlayableSystem : FSystem {
    private readonly Family _InVesselPointerOver = FamilyManager.getFamily(
        new AllOfComponents(typeof(Playable), typeof(PointerOver), typeof(Origin)),
        new NoneOfLayers(11) // Immuno layer
    );

    private readonly Family _EndWaypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(EndNode))
    );

    private readonly Family _Waypoints = FamilyManager.getFamily(
        new AllOfComponents(typeof(Node))
    );

    private readonly Family _ToRepop = FamilyManager.getFamily(
       new AllOfComponents(typeof(Macrophage)),
       new NoneOfComponents(typeof(PathFollower)),
       new AnyOfLayers(11)
   );

    private readonly Family _StartTrigger = FamilyManager.getFamily(
        new AllOfComponents(typeof(StartLoopTrigger))
    );


    private readonly Family _YellowPages = FamilyManager.getFamily(
        new AllOfComponents(typeof(YellowPageComponent))
    );

    // Use to process your families.
    protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _InVesselPointerOver)
        {
            ProcessInVesselPointerOver();
            ProcessUseless();
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
                    foreach(PairEStatTrackedEntityInt trackedEntity in Global.data.trackedEntities)
                    {
                        if (trackedEntity.a == playable.trackedEntity)
                        {
                            trackedEntity.b++;
                            break;
                        }
                    }
                }
                GameObject target = PathSystem.GetClosestWaypoint(go, _Waypoints);

                GameObjectManager.addComponent(go, typeof(PathFollower), new
                {
                    destination = PathSystem.ComputeDestination(target.transform.position, _EndWaypoints)
                });  // Compute destination (the closest one from start waypoint)

                move.target = target.transform.position; // Move cell to closest waypoint
            }
        }
    }

    private void ProcessUseless()
    {
        StartLoopTrigger start = _StartTrigger.First().GetComponent<StartLoopTrigger>();
        foreach (GameObject go in _ToRepop)
        {
            GameObjectManager.addComponent<Removed>(go);

            Origin origin = go.GetComponent<Origin>();
            YellowPageComponent yp = _YellowPages.First().GetComponent<YellowPageComponent>();
            start.deckPool.Add(YellowPageUtils.GetSourceObject(yp, origin.sourceObjectKey));
        }
    }
}