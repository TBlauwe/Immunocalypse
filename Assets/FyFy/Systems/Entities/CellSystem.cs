using UnityEngine;
using FYFY;

/// <summary>
///     This system manages all cell in the scene. The workflow is the following :
///     <list type="number">
///         <item>Cell are working with an internal FSM with 3 states : HEALTHY, INFECTED and DEAD</item>
///         <item>Infection by pathogeness are treated in the InfectionSystem</item>
///         <item>When the cell is infected, each entered pathogene is stored and will replicate, consuming 1 resource</item>
///         <item>When the cell is about to die, state is set to DEAD. This result in creating a factory which will
///         produce all replicated pathogenes in a small amount of time.</item>
///     </list>
/// </summary>
public class CellSystem : FSystem {

    // Default rate for the factory when the cell dies
    private static readonly float DEFAULT_RATE = 0.1f;

    // Eatable layer for infected cell
    private static readonly int INFECTED_EATABLE_LAYER = 2;

    // This family gather all known cells
    private readonly Family _cells = FamilyManager.getFamily(
        new AllOfComponents(typeof(Cell)),
        new NoneOfComponents(typeof(Removed))
    );

	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject entity in _cells)
        {
            Cell cell = entity.GetComponent<Cell>();
            
            // If a pathogene entered in the cell, the cell state is changed to INFECTED
            // We also add components that will indicate the cell can be eaten by macrophages
            if (cell.state.Equals(CellState.HEALTHY) && cell.infections.Count > 0)
            {
                cell.state = CellState.INFECTED;
                GameObjectManager.addComponent<Eatable>(entity, new {eatableLevel = INFECTED_EATABLE_LAYER});
            }

            // If the cell is infected, we have to replicate all pathogenes, resulting in consuming resources
            // Resources are represented as the cell's health
            WithHealth healthComponent = entity.GetComponent<WithHealth>();
            if (cell.state.Equals(CellState.INFECTED))
            {
                int i = 0;
                
                while (healthComponent.health > 1 && i < cell.infections.Count)
                {
                    GameObject go = cell.infections[i].prefab;
                    Infectious infectious = go.GetComponent<Infectious>();

                    // Decrease cooldown
                    infectious.cooldown -= Time.deltaTime;
                    if (infectious.cooldown < 0)
                    {
                        // Replicate once
                        cell.infections[i].nb++;
                        cell.infections[i].originalNb++;
                        healthComponent.health -= infectious.replicationCost;

                        // Reset cooldown
                        infectious.cooldown = infectious.replicationTime;
                    }
                                        
                    /*
                    int potentialNbOfPathogenes = Mathf.CeilToInt(cell.infections[i].originalNb  * Time.deltaTime / 2);
                    int consumedResources = (int) Mathf.Min(potentialNbOfPathogenes, healthComponent.health - 1);
                    cell.infections[i].originalNb += Mathf.FloorToInt(Mathf.Log(consumedResources));
                    cell.infections[i].nb = cell.infections[i].originalNb;
                    healthComponent.health -= consumedResources;
                    */
                    ++i;
                }
                
                // If the is no more resources, the cell is about to die
                if (healthComponent.health <= 0)
                {
                    cell.state = CellState.DEAD;
                }
            }

            // The cell is about to die, the factory is created
            if (cell.state.Equals(CellState.DEAD))
            {
                // Create the factory
                GameObject obj = new GameObject();
                obj.transform.position = entity.transform.position;

                // Configure the factory
                Factory factory = obj.AddComponent<Factory>();
                factory.rate = DEFAULT_RATE;
                factory.entries = cell.infections;
                factory.useRandomSpawning = true;

                // Bind it to FYFY
                GameObjectManager.bind(obj);

                // Set all prefab as children of the created factory : they will be removed when the factory will deseapper too.
                foreach (FactoryEntry entry in cell.infections)
                {
                    entry.prefab.transform.SetParent(obj.transform);
                }

                // Die
                GameObjectManager.addComponent<Removed>(entity);
            }
        }
	}
}