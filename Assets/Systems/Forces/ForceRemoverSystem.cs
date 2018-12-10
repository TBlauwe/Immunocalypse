using UnityEngine;
using FYFY;

/// <summary>
/// Remove all force-related components to matching entities. Using RemoveForces component.
/// </summary>
public class ForceRemoverSystem : FSystem {
    // Get all entities with the RemoveForces component
    private readonly Family _managers = FamilyManager.getFamily(new AllOfComponents(typeof(RemoveForces), typeof(ForceManager)));
    private readonly Family _managed = FamilyManager.getFamily(new AllOfComponents(typeof(RemoveForces), typeof(ForceManaged)));

	protected override void onProcess(int currentFrame) {
        foreach (GameObject entity in _managed)
        {
            HandleEntity(entity);
        }
        foreach (GameObject entity in _managers)
        {
            HandleEntity(entity);
        }
	}

    private void HandleEntity(GameObject entity)
    {
        // Here we are making some force management
        // Have the object a ForceManaged component ?
        ForceManaged managed = entity.GetComponent<ForceManaged>();
        if (managed != null)
        {
            if (managed.parent != null)
            {
                ForceCreator[] forceCreators = managed.parent.GetComponents<ForceCreator>();
                foreach (ForceCreator forceCreator in forceCreators)  // Put the ForceCreator component if available
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

                // Could have many ForceCreator
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

        // Finally, delete RemoveForces component
        foreach (RemoveForces rm in entity.GetComponents<RemoveForces>()) {
            GameObjectManager.removeComponent(rm);
        }
    }
}