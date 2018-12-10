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
                    if (target.activeSelf && target.GetComponent<Removed>() == null && target.GetComponent<Infectious>() != null && target.GetComponent<RemoveForces>() == null)
                    {
                        FactoryEntry infection = new FactoryEntry(target)
                        {
                            originalNb = 2,
                            nb = 2
                        };

                        if (target.GetComponent<ForceManaged>() != null)
                        {
                            //HandleForceManagement(target);
                            GameObjectManager.addComponent<RemoveForces>(target);
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
            GameObjectManager.addComponent<RemoveForces>(entity);
        }
	}
}