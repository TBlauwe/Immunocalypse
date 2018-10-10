using UnityEngine;
using FYFY;
using System.Collections.Generic;

public class FactorySystem : FSystem {
    private readonly Family _factories = FamilyManager.getFamily(new AllOfComponents(
        typeof(Factory)
    ));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _factories)
        {
            Factory factory = go.GetComponent<Factory>();

            factory.remainingTime -= Time.deltaTime;
            
            if (factory.remainingTime <= 0)
            {
                // Reset factory remainingTime according to the rate
                factory.remainingTime = factory.rate;

                // Select the prefab that wil be instanciated according provided probabilities
                List<float> cumulative = GetCumulativeProbabilities(factory.probabilities);

                // get a random number
                float number = Random.value;

                // Find index
                int index = FindIndex(number, cumulative);

                // Instantiate object and set its position
                GameObject unit = Object.Instantiate(factory.prefabs[index]);
                unit.transform.position = factory.transform.position;

                // Bind to FYFY
                GameObjectManager.bind(unit);
            }
        }
	}

    private List<float> GetCumulativeProbabilities(float[] initial)
    {
        List<float> cumulative = new List<float>();
        foreach (float proba in initial)
        {
            if (cumulative.Count > 0)
            {
                cumulative.Add(cumulative[cumulative.Count - 1] + proba);
            }
            else
            {
                cumulative.Add(proba);
            }
        }
        return cumulative;
    }

    private int FindIndex(float number, List<float> cumulative)
    {
        int i = 0;
        while (i < cumulative.Count && number < cumulative[i])
        {
            ++i;
        }
        return (i == 0) ? i : i - 1;
    }
}