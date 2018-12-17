using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;
using System.Collections.Generic;

public class ForceManagementSystem : FSystem {
    // Learders management
    private readonly Family _potentialLeaders = FamilyManager.getFamily(
        new NoneOfComponents(typeof(ForceManaged), typeof(Removed), typeof(RemoveForces), typeof(Cell)), //until I figure a way to do it
        new AllOfComponents(typeof(Triggered3D), typeof(ForceCreator)),
        new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );

    // Use to process your families.
    protected override void onProcess(int familiesUpdateCount) {
        ManageForceLeaders();
    }

    /// <summary>
    /// Designed to manage forces leaders.
    /// </summary>
    private void ManageForceLeaders()
    {
        // Should the entity be a ForceLeader ?
        List<GameObject> forbidden = new List<GameObject>();
        foreach (GameObject entity in _potentialLeaders)
        {
            if (!forbidden.Contains(entity)) // Because FYFY adds component at the next tick
            {
                // At this point, entity is a leader
                List<GameObject> newChildren = new List<GameObject>();

                // Cannot be considered as availabe entity for others
                forbidden.Add(entity);

                //foreach (GameObject target in entity.GetComponent<Triggered3D>().Targets)
                GameObject[] targets = entity.GetComponent<Triggered3D>().Targets;
                for (int i = 0; i < targets.Length; ++i)
                {
                    GameObject target = targets[i];

                    ForceManaged managed = target.GetComponent<ForceManaged>();
                    ForceManager otherManager = target.GetComponent<ForceManager>();

                    ForceCreator creator = target.GetComponent<ForceCreator>();
                    ForceCreator leaderCreator = entity.GetComponent<ForceCreator>();

                    Removed removed = target.GetComponent<Removed>();
                    //RemoveForces rforces = target.GetComponent<RemoveForces>();

                    if (creator != null && leaderCreator.forceLayerMask == creator.forceLayerMask && managed == null
                        && otherManager == null && !forbidden.Contains(target) && removed == null)// && rforces == null)
                    {
                        forbidden.Add(target);
                        newChildren.Add(target);
                        try
                        {
                            GameObjectManager.addComponent<ForceManaged>(target, new { parent = entity });
                            GameObjectManager.addComponent<SpringJoint>(target, new { connectedBody = entity.GetComponent<Rigidbody>() });
                            GameObjectManager.removeComponent<ForceCreator>(target);
                            GameObjectManager.removeComponent<SubjectToForces>(target);
                        }
                        catch (DestroyedGameObjectException e)
                        {
                            Debug.LogException(e);// ignore, happen when a macrophage just ate the bacteria
                        }
                    }
                }

                // Add new Children to the leader
                ForceManager manager = entity.GetComponent<ForceManager>();
                if (manager == null)
                {
                    GameObjectManager.addComponent<ForceManager>(entity, new { children = newChildren });
                }
                else
                {
                    manager.children.AddRange(newChildren);
                }
            }
        }
    }
}