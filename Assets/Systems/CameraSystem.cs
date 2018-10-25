using UnityEngine;
using FYFY;

public class CameraSystem : FSystem {

    // =============================
    // ========== GENERAL ==========
    // =============================
    private Family          _cameraGO = FamilyManager.getFamily(new AllOfComponents(typeof(Camera), typeof(CameraSettings)));

    private GameObject      pivot;
    private GameObject      cameraGO;
    private Camera          camera;
    private CameraSettings  cameraSettings;
    private CAMERA_MODE     cameraMode; // Used to detect camera mode changes

    // =====================================
    // ========== INSPECTION MODE ==========
    // =====================================
    private Family          _galleryModelGO = FamilyManager.getFamily(new AllOfComponents(typeof(GalleryModel)));

    // ===================================
    // ========== GAMEPLAY MODE ==========
    // ===================================
    private Family          _levelGO = FamilyManager.getFamily(new AllOfComponents(typeof(LevelSettings)));
    private LevelSettings   levelSettings;

    // =================================
    // ========== CONSTRUCTOR ==========
    // =================================
    public CameraSystem()
    {
        // ---------- VARIABLES INITIALIZATION ----------
        cameraGO        = _cameraGO.First();
        pivot           = cameraGO.transform.parent.gameObject;
        camera          = cameraGO.GetComponent<Camera>();
        cameraSettings  = cameraGO.GetComponent<CameraSettings>();
        cameraMode      = cameraSettings.mode;

        // ---------- SETUP BASED ON MODE ----------

        switch (cameraMode)
        {
            case CAMERA_MODE.FIXED:
                break;
            case CAMERA_MODE.GAMEPLAY:
                if(_levelGO.Count == 0) { throw new ArgumentNullException("No level in scene, cannot setup camera accordingly"); }
                setupCameraForGameplay(true);
                break;
            case CAMERA_MODE.INSPECTION:
                if(_galleryModelGO.Count == 0) { throw new ArgumentNullException("No gallery models, cannot setup camera accordingly"); }
                break;
            case CAMERA_MODE.GALLERY:
                if(_galleryModelGO.Count == 0) { throw new ArgumentNullException("No gallery models, cannot setup camera accordingly"); }
                break;
        }
    }

    // ================================
    // ========== MAIN LOOPS ==========
    // ================================
	protected override void onProcess(int familiesUpdateCount)
    {
        switch (cameraSettings.mode)
        {
            case CAMERA_MODE.FIXED:
                break;
            case CAMERA_MODE.GAMEPLAY:
                updateCameraForGameplay();
                break;
            case CAMERA_MODE.INSPECTION:
                break;
            case CAMERA_MODE.GALLERY:
                break;
        }

	}
    // ===========================
    // ========== SETUP ==========
    // ===========================
    private void setupCameraForGameplay(bool saveAsDefault) {
        levelSettings   = _levelGO.First().GetComponent<LevelSettings>();

        Vector3 size = levelSettings.size; 

        setPivotPosition(levelSettings.center);
        setCameraPosition(new Vector3(size.x / 2.5f, size.x / 2.5f, 0.0f), saveAsDefault);
        setCameraRotation(new Vector3(45, -90), saveAsDefault);
        setCameraZoom(size.x / 2, saveAsDefault);

        cameraSettings.ZoomMax = size.x/2;
    }


    // ============================
    // ========== UPDATE ==========
    // ============================

    private void updateCameraForGameplay()
    {
        // UPDATE TRANSLATION
        Vector3 translation = GetInputTranslationDirection();
        if (!translation.Equals(Vector3.zero)) { updateTranslation(translation); }

        // UPDATE ROTATION
        float rotationDelta = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(1) && rotationDelta != 0) { updateRotation(rotationDelta); }

        // UPDATE ZOOM 
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
		if (zoomDelta != 0f) { updateZoom(zoomDelta); }

        // Reset position
        if (Input.GetKey(KeyCode.R))
        {
            cameraGO.transform.position     = cameraSettings.defaultPosition;
            cameraGO.transform.eulerAngles  = cameraSettings.defaultRotation;
            camera.orthographicSize         = cameraSettings.defaultSize;
        }
    }

    // =============================
    // ========== PRIVATE ==========
    // =============================
    private void setPivotPosition(Vector3 position) { pivot.transform.position = position; }

    private void setCameraPosition(Vector3 position, bool saveAsDefault) {
        if (saveAsDefault)
        {
            cameraSettings.defaultPosition = position;
        }
        cameraGO.transform.position = position;
    }

    private void setCameraRotation(Vector3 rotation, bool saveAsDefault) {
        if (saveAsDefault)
        {
            cameraSettings.defaultRotation = rotation;
        }
        cameraGO.transform.eulerAngles = rotation;
    }

    private void setCameraZoom(float zoom, bool saveAsDefault) {
        if (saveAsDefault)
        {
            cameraSettings.defaultSize = zoom;
        }
        camera.orthographicSize = zoom;
    }


    // ============================
    // ========== HELPER ==========
    // ============================
    private bool hasStateChanged()
    {
        bool stateChanged=false;

        if(cameraMode != cameraSettings.mode)
        {
            cameraMode = cameraSettings.mode;
            stateChanged = true;
        }
        return stateChanged; 
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

    private float smoothZoom(float from, float to, float speed)
    {
        Debug.Log(cameraSettings.ZoomMin);
        Debug.Log(cameraSettings.ZoomMax);
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