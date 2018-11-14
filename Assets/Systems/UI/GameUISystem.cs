using UnityEngine;
using FYFY;

public class GameUISystem : FSystem {
    private Family displayableHealthFamily = FamilyManager.getFamily(new AllOfComponents(typeof(DisplayableHealth)));
    
    public GameUISystem()
    {
        foreach(GameObject gameObject in displayableHealthFamily)
        {
            onNewObjectInFamily(gameObject);
        }
        displayableHealthFamily.addEntryCallback(onNewObjectInFamily);
    }

    private void onNewObjectInFamily(GameObject gameObject)
    {
        WithHealth health = gameObject.GetComponent<WithHealth>();
        DisplayableHealth displayableHealth = gameObject.GetComponent<DisplayableHealth>();
        displayableHealth.slider.maxValue = health.health;
        displayableHealth.slider.value = health.health;
    }

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
        foreach(GameObject gameObject in displayableHealthFamily)
        {
            WithHealth health = gameObject.GetComponent<WithHealth>();
            DisplayableHealth displayableHealth = gameObject.GetComponent<DisplayableHealth>();
            displayableHealth.slider.value = health.health;
        }
	}
}