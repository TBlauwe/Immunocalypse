using UnityEngine;
using FYFY;

public class CameraScrollingSystem : FSystem {

    private Family cameraFamily = FamilyManager.getFamily(new AllOfComponents(typeof(Camera), typeof(CameraSettings)));
    
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
        foreach(GameObject gameObject in this.cameraFamily)
        {
            CameraSettings cameraSettings = gameObject.GetComponent<CameraSettings>();
            Vector3 translation = new Vector3(0, 1, 0);
            float axis = Input.GetAxis("Mouse ScrollWheel");
            if (axis < 0)
            {
                translation *= -1;
            }

            if (axis != 0)
            {
                gameObject.transform.Translate(translation * cameraSettings.scrollSpeed);
            }
            Vector3 position = gameObject.transform.position;
            if(position.x <= cameraSettings.minXScrollingPosition)
            {
                position.x = cameraSettings.minXScrollingPosition;
            }
            if(position.x >= cameraSettings.maxXScrollingPosition)
            {
                position.x = cameraSettings.maxXScrollingPosition;
            }
            gameObject.transform.position = position;
        }
	}
}