using UnityEngine;
using FYFY;

public class EphemeralSystem : FSystem {
    Family family = FamilyManager.getFamily(new AllOfComponents(typeof(Ephemeral)));
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){

    }

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach(GameObject go in family)
        {
            WithHealth health = go.GetComponent<WithHealth>();
            Ephemeral ephemeral = go.GetComponent<Ephemeral>();
            if (health.health <= 0) continue;
            Debug.Log(health.health);
            if (ephemeral.elapsedTime == 0)
            {
                ephemeral.startingHealth = health.health;
            }
            ephemeral.elapsedTime += Time.deltaTime;
            if(ephemeral.degressiveHealth)
            {
                health.health -= (Time.deltaTime * ephemeral.startingHealth) / ephemeral.lifetime;
            }
            if(ephemeral.elapsedTime > ephemeral.lifetime)
            {
                health.health = 0;
            }
        }
	}
}