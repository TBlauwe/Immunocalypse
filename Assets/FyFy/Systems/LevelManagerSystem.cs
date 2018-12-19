using UnityEngine;
using FYFY;

public class LevelManagerSystem : FSystem {
    Family levelsFamily = FamilyManager.getFamily(new AllOfComponents(typeof(Level)));
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
        foreach (GameObject gameObject in levelsFamily)
        {
            Level level = gameObject.GetComponent<Level>();
            level.runningTime += Time.deltaTime;
            foreach (Wave wave in level.waves)
            {
                if(!wave.launched && level.runningTime>=wave.launchingTime)
                {
                    wave.launch(gameObject);
                }
            }
        }
    }
}