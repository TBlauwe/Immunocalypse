using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;

public class RandomEffectSystem : FSystem {

    private readonly Family _RandomGO = FamilyManager.getFamily(
        new AllOfComponents(typeof(RandomEffect)),
        new NoneOfComponents(typeof(Removed))
    );

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _RandomGO)
        {
            RandomEffect randomEffect = go.GetComponent<RandomEffect>();

            if (go.GetComponent<PointerOver>() != null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Clicked");
                Object.Instantiate(randomEffect.onClickGO, go.GetComponent<Collider>().bounds.center, go.transform.rotation);
                GameObjectManager.addComponent<Removed>(go);
            }
        }
	}
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}
}