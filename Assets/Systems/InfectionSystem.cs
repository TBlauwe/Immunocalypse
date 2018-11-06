﻿using UnityEngine;
using FYFY;
using FYFY_plugins.CollisionManager;

public class InfectionSystem : FSystem {
    private readonly Family _cells = FamilyManager.getFamily(new AllOfComponents(typeof(Cell), typeof(InCollision3D)));

	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject entity in _cells)
        {
            Cell cell = entity.GetComponent<Cell>();
            if (!cell.state.Equals(CellState.DEAD))
            {
                GameObject[] targets = entity.GetComponent<InCollision3D>().Targets;
                foreach (GameObject target in targets)
                {
                    if (target.tag == "pathogene" && target.activeSelf)
                    {
                        FactoryEntry infection = new FactoryEntry(target)
                        {
                            originalNb = 2,
                            nb = 2
                        };

                        cell.infections.Add(infection);
                        target.SetActive(false);
                        target.transform.parent = entity.transform;
                    }
                }
            }
        }
	}
}