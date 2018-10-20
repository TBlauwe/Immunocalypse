using UnityEngine;
using FYFY;

public class CameraSystem : FSystem {

    private Family _cameraGO = FamilyManager.getFamily(new AllOfComponents(typeof(Camera), typeof(CameraSettings)));
    private Family _levelGO = FamilyManager.getFamily(new AllOfComponents(typeof(LevelSettings)));

    private GameObject      pivot;
    private GameObject      cameraGO;
    private Camera          camera;
    private CameraSettings  cameraSettings;
    private LevelSettings   levelSettings;

    public CameraSystem()
    {
        cameraGO        = _cameraGO.First();
        camera          = cameraGO.GetComponent<Camera>();
        cameraSettings  = cameraGO.GetComponent<CameraSettings>();
        levelSettings   = _levelGO.First().GetComponent<LevelSettings>();

        if(cameraGO.transform.parent == null){ pivot = new GameObject(); }
        else { pivot = cameraGO.transform.parent.gameObject; }

        setupCamera();
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

        updateCamera();
	}

    private void setupCamera() {
    
        // --------------------------
        // --- SET PIVOT POSITION ---
        // --------------------------
        pivot.transform.position = levelSettings.center;

        // ---------------------------
        // --- SET CAMERA POSITION ---
        // ---------------------------
        cameraGO.transform.position = Vector3.zero;

        Vector3 size = levelSettings.size; 

        float x = size.x / 3;
        float y = size.x / 3;
        float z = 0.0f;

        Vector3 position = new Vector3(x, y, z);
        cameraGO.transform.position = position;


        // --------------------
        // --- SET ROTATION ---
        // --------------------
        Vector3 rotation =  new Vector3(45, -90);
        cameraGO.transform.eulerAngles = rotation;

        // --------------------
        // --- SET DEFAULTS ---
        // --------------------
        cameraSettings.ZoomMax = size.x / 2;
        camera.orthographicSize = cameraSettings.ZoomMax;

        cameraSettings.defaultSize = camera.orthographicSize;
        cameraSettings.defaultPosition = position;
        cameraSettings.defaultRotation = rotation;
    }

    private void updateTranslation(Vector3 translation)
    {
        Vector3 rotatedTranslation = Quaternion.Euler(  camera.transform.eulerAngles.x,
                                                        camera.transform.eulerAngles.y,
                                                        camera.transform.eulerAngles.z) * translation;

        camera.transform.position = smoothTranslation(  camera.transform.position,
                                                        camera.transform.position + rotatedTranslation,
                                                        cameraSettings.MoveSpeed);
    }

    private void updateRotation(float delta)
    {
        delta *= Time.deltaTime * cameraSettings.RotationSpeed;
        cameraGO.transform.RotateAround(pivot.transform.position, Vector3.up, delta);
    }

    private void updateZoom(float delta)
    {
        float zoom = camera.orthographicSize - delta;
        camera.orthographicSize = smoothZoom(camera.orthographicSize, zoom, cameraSettings.ZoomSpeed);
    }

    private void updateCamera()
    {
        // UPDATE TRANSLATION
        Vector3 translation = GetInputTranslationDirection();
        if (!translation.Equals(Vector3.zero)) { updateTranslation(translation);  }

        // UPDATE ROTATION
        float rotationDelta = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(1) && rotationDelta != 0) { updateRotation(rotationDelta); }

        // UPDATE ZOOM 
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
		if (zoomDelta != 0f) { updateZoom(zoomDelta);  }

        // Reset position
        if (Input.GetKey(KeyCode.R))
        {
            cameraGO.transform.position     = cameraSettings.defaultPosition;
            cameraGO.transform.eulerAngles  = cameraSettings.defaultRotation;
            camera.orthographicSize         = cameraSettings.defaultSize;
        }
    }

    private float smoothZoom(float from, float to, float speed)
    {
        return Mathf.Clamp(Mathf.SmoothStep(from, to, Time.deltaTime * speed), cameraSettings.ZoomMin, cameraSettings.ZoomMax);
    }

    private Vector3 smoothTranslation(Vector3 from, Vector3 to, float speed)
    {
        return Vector3.Lerp(from, to, Time.deltaTime * speed);
    }

    private Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.Z))
        {
            direction += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        return direction;
    }
}