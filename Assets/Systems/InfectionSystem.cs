using UnityEngine;
using FYFY;
using FYFY_plugins.CollisionManager;
using System.Collections.Generic;

public class InfectionSystem : FSystem {
    private readonly Family _cells = FamilyManager.getFamily(new AllOfComponents(typeof(Cell), typeof(InCollision3D)));

	protected override void onProcess(int familiesUpdateCount) {

        List<GameObject> leaders = new List<GameObject>();

        foreach (GameObject entity in _cells)
        {
            Cell cell = entity.GetComponent<Cell>();
            if (!cell.state.Equals(CellState.DEAD))
            {
                GameObject[] targets = entity.GetComponent<InCollision3D>().Targets;
                foreach (GameObject target in targets)
                {
                    if (target.tag == "pathogene" && target.activeSelf && target.GetComponent<Removed>() == null)
                    {
                        FactoryEntry infection = new FactoryEntry(target)
                        {
                            originalNb = 2,
                            nb = 2
                        };

                        if (target.GetComponent<ForceManaged>() != null)
                        {
                            HandleForceManagement(target);
                        } else if (target.GetComponent<ForceManager>() != null)
                        {
                            leaders.Add(target);
                        }
                        cell.infections.Add(infection);
                        target.SetActive(false);
                        target.transform.parent = entity.transform;
                    }
                }
            }
        }

        foreach (GameObject entity in leaders)
        {
            HandleForceManagement(entity);
        }
	}

    private void HandleForceManagement(GameObject entity)
    {
        // Here we are making some force management
        // Have the object a ForceManaged component ?
        ForceManaged managed = entity.GetComponent<ForceManaged>();
        if (managed != null)
        {
            if (managed.parent != null)
            {
                ForceCreator forceCreator = managed.parent.GetComponent<ForceCreator>();
                if (forceCreator != null)  // Put the ForceCreator component if available
                {
                    GameObjectManager.addComponent<ForceCreator>(entity, new { forceLayerMask = forceCreator.forceLayerMask });
                }
                SubjectToForces subjectToForces = managed.parent.GetComponent<SubjectToForces>();
                if (subjectToForces != null)
                {
                    GameObjectManager.addComponent<SubjectToForces>(
                        entity,
                        new { appliedForces = subjectToForces.appliedForces, speed = subjectToForces.speed }
                    );
                }
                ForceManager parentManager = managed.parent.GetComponent<ForceManager>();
                if (parentManager != null)
                {
                    parentManager.children.Remove(entity);
                }
            }
            GameObjectManager.removeComponent<ForceManaged>(entity);
        }

        // Have the game object a ForceManager component ?
        ForceManager manager = entity.GetComponent<ForceManager>();
        if (manager != null)
        {
            foreach (GameObject child in manager.children)
            {
                GameObjectManager.removeComponent<SpringJoint>(child);
                GameObjectManager.removeComponent<ForceManaged>(child);

                GameObjectManager.addComponent<ForceCreator>(
                    child,
                    new { forceLayerMask = entity.GetComponent<ForceCreator>().forceLayerMask }
                );

                SubjectToForces subjectToForces = entity.GetComponent<SubjectToForces>();
                GameObjectManager.addComponent<SubjectToForces>(
                        child,
                        new { appliedForces = subjectToForces.appliedForces, speed = subjectToForces.speed }
                    );
            }
            GameObjectManager.removeComponent<ForceManager>(entity);
        }

        // If a SprintJoint is found, delete it
        SpringJoint joint = entity.GetComponent<SpringJoint>();
        if (joint != null)
        {
            GameObjectManager.removeComponent<SpringJoint>(entity);
        }
    }
}