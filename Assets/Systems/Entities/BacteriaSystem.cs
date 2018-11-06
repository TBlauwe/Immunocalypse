using UnityEngine;
using FYFY;

public class BacteriaSystem : FSystem {
    private readonly Family bacterias = FamilyManager.getFamily(new AllOfComponents(typeof(Bacteria)));

	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject entity in bacterias)
        {
            Bacteria bacteria = entity.GetComponent<Bacteria>();

            if (Random.value <= bacteria.duplicationProbability)
            {
                // Create clone
                GameObject clone = Object.Instantiate(entity);
                clone.transform.position = entity.transform.position;

                // Bind it to FYFY
                GameObjectManager.bind(clone);
            }
        }
    }
}