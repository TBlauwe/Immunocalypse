using UnityEngine;
using FYFY;

public class DontDestroyOnLoadSystem : FSystem {

    private Family family = FamilyManager.getFamily(new AllOfComponents(typeof(DontDestroyOnLoad)));


    public DontDestroyOnLoadSystem()
    {
        foreach (GameObject go in family)
        {
            onFamilyEnter(go);
        }
        family.addEntryCallback(onFamilyEnter);
    }

    public void onFamilyEnter(GameObject gameObject)
    {
        GameObjectManager.dontDestroyOnLoadAndRebind(gameObject);
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
	}


}