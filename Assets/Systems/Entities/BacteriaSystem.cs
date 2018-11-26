using UnityEngine;
using FYFY;
using System.Collections.Generic;
using FYFY_plugins.TriggerManager;

public class BacteriaSystem : FSystem {
    // All bacterias in the game
    private readonly Family bacterias = FamilyManager.getFamily(
        new AllOfComponents(typeof(Bacteria)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF),
        new NoneOfComponents(typeof(Removed))
    );

    private readonly Family _potentialLeaders = FamilyManager.getFamily(
        new NoneOfComponents(typeof(ForceManaged), typeof(Removed)), new AllOfComponents(typeof(Bacteria), typeof(Triggered3D)),
        new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );

    protected override void onProcess(int familiesUpdateCount) {
        // Manage force leaders
        ManageForceLeaders();

        // Replication

        /*foreach (GameObject entity in bacterias)
        {
            Bacteria bacteria = entity.GetComponent<Bacteria>();

            // Bacteria replication
            if (Random.value <= bacteria.duplicationProbability)
            {
                // Create clone
                GameObject clone = Object.Instantiate(entity);
                clone.transform.position = entity.transform.position;

                // Bind it to FYFY
                GameObjectManager.bind(clone);
            }
        }*/
    }

    private void ManageForceLeaders()
    {
        // Should the bacteria be a ForceLeader ?
        List<GameObject> forbidden = new List<GameObject>();
        foreach (GameObject entity in _potentialLeaders) { 
            if (!forbidden.Contains(entity)) // Because FYFY adds component at the next tick
            {
                // At this point, bacteria is a leader
                List<GameObject> newChildren = new List<GameObject>();

                // Cannot be considered as availabe bacteria for others
                forbidden.Add(entity);

                //foreach (GameObject target in entity.GetComponent<Triggered3D>().Targets)
                GameObject[] targets = entity.GetComponent<Triggered3D>().Targets;
                for (int i = 0; i < targets.Length; ++i)
                {
                    GameObject target = targets[i];

                    Bacteria otherBacteria = target.GetComponent<Bacteria>();
                    ForceManaged managed = target.GetComponent<ForceManaged>();
                    ForceManager otherManager = target.GetComponent<ForceManager>();

                    if (otherBacteria != null && managed == null && otherManager == null && !forbidden.Contains(target)) // Not managed but also not a leader
                    {
                        forbidden.Add(target);
                        newChildren.Add(target);
                        try
                        {
                            GameObjectManager.addComponent<ForceManaged>(target, new { parent = entity });
                            GameObjectManager.addComponent<SpringJoint>(target, new { connectedBody = entity.GetComponent<Rigidbody>() });
                            GameObjectManager.removeComponent<ForceCreator>(target);
                            GameObjectManager.removeComponent<SubjectToForces>(target);
                        } catch (DestroyedGameObjectException e) {
                            Debug.LogException(e);// ignore, happen when a macrophage just ate the bacteria
                        }
                    }
                }

                // Add new Children to the leader
                ForceManager manager = entity.GetComponent<ForceManager>();
                if (manager == null)
                {
                    GameObjectManager.addComponent<ForceManager>(entity, new { children=newChildren  });
                }
                else
                {
                    manager.children.AddRange(newChildren);
                }
            }
        }
    }
}