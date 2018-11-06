using UnityEngine;
using FYFY;
using System.Collections.Generic;

public class FactorySystem : FSystem
{
    private readonly Family _factories = FamilyManager.getFamily(new AllOfComponents(
        typeof(Factory)
    ));

    // Use to process your families.
    protected override void onProcess(int familiesUpdateCount)
    {
        foreach (GameObject go in _factories)
        {
            Factory factory = go.GetComponent<Factory>();
            
            // Is the factory working ? Should it be destroyed ?
            if (!AtLeastOneThingToInstanciate(factory.entries))
            {
                if (factory.destroyWhenFinished)
                {
                    // Tmp
                    GameObjectManager.unbind(go);
                    Object.Destroy(go);
                }
                continue;
            }
            if (factory.paused) continue;

            // Update remaining time and skip instanciation process if we still have to wait
            factory.remaining -= Time.deltaTime;
            if (factory.remaining > 0) continue;

            // Select the entity that will be created (according to factory's mode)
            FactoryEntry entry = null;
            if (factory.useRandomSpawning)
            {
                do
                {
                    entry = factory.entries[Random.Range(0, factory.entries.Count)];
                } while (entry.nb == 0) ;
            }
            else
            {
                int i = 0;
                do
                {
                    entry = factory.entries[i];
                    ++i;
                } while (entry.nb == 0);
            }

            //if (entry.prefab == null) continue; //FIXME: why is it required ?

            // Instanciate the GameObject
            GameObject clone = Object.Instantiate(entry.prefab);
            clone.SetActive(true);
            GameObjectManager.bind(clone);

            // Set GameObject's position
            clone.transform.position = go.transform.position;

            // Decrease the number of remaining object to instanciate (for this prefab)
            --entry.nb;

            // Reset remaining time to rate
            factory.remaining = factory.rate;
        }
    }

    private bool AtLeastOneThingToInstanciate(List<FactoryEntry> entries)
    {
        bool found = false;
        int i = 0;
        while (!found && i < entries.Count)
        {
            found = entries[i].nb > 0;
            ++i;
        }
        return found;
    }
}