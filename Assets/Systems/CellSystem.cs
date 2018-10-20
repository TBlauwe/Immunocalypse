using UnityEngine;
using FYFY;

public class CellSystem : FSystem {

    private static readonly float DEFAULT_RATE = 0.1f;

    private readonly Family _cells = FamilyManager.getFamily(new AllOfComponents(typeof(Cell)));

	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject entity in _cells)
        {
            Cell cell = entity.GetComponent<Cell>();
            

            if (cell.state.Equals(CellState.HEALTHY) && cell.infections.Count > 0)
            {
                cell.state = CellState.INFECTED;
            }

            WithHealth healthComponent = entity.GetComponent<WithHealth>();
            if (cell.state.Equals(CellState.INFECTED))
            {
                int i = 0;

                while (healthComponent.health > 1 && i < cell.infections.Count)
                {
                    int potentialNbOfPathogenes = Mathf.CeilToInt(cell.infections[i].originalNb  * Time.deltaTime);
                    int consumedResources = (int) Mathf.Min(potentialNbOfPathogenes, healthComponent.health - 1);
                    cell.infections[i].originalNb += consumedResources;
                    cell.infections[i].nb = cell.infections[i].originalNb;
                    healthComponent.health -= consumedResources;
                    ++i;
                }
                if (healthComponent.health <= 1)
                {
                    healthComponent.health = 1;
                    cell.state = CellState.DEAD;
                }
            }

            if (cell.state.Equals(CellState.DEAD))
            {
                // Tell Unity to destroy prefabs later
                int sum = 0;
                foreach (FactoryEntry entry in cell.infections)
                {
                    sum += entry.originalNb;
                }
                foreach (FactoryEntry entry in cell.infections)
                {
                    Object.Destroy(entry.prefab, 3 * sum * DEFAULT_RATE);
                }

                // Create the factory
                GameObject obj = new GameObject();
                obj.transform.position = entity.transform.position;

                Factory factory = obj.AddComponent<Factory>();
                factory.rate = DEFAULT_RATE;
                factory.entries = cell.infections;
                factory.useRandomSpawning = true;
                factory.destroyWhenFinished = true;

                GameObjectManager.bind(obj);

                // Die
                healthComponent.health = 0;
            }
        }
	}
}