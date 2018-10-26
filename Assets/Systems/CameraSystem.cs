using System.Collections.Generic;
using System.Linq;
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

    // ==================================
    // ========== GALLERY MODE ==========
    // ==================================
    private Family          _galleryManagerGO = FamilyManager.getFamily(new AllOfComponents(typeof(GalleryManager)));
    private GalleryManager  galleryManager;
    private int             currentRank=-1;
    private int             maxRank;

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
                break;
            case CAMERA_MODE.GALLERY:
                if(_galleryManagerGO.Count == 0) { throw new ArgumentNullException("No gallery manager in scene, cannot setup camera accordingly"); }
                setupCameraForGallery();
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
                updateCameraForGallery();
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

    private void setupCameraForGallery()
    {
        galleryManager = _galleryManagerGO.First().gameObject.GetComponent<GalleryManager>();
        maxRank = galleryManager.galleryModels.Count;
        focus(0);
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

    private void updateCameraForGallery()
    {
        // UPDATE ROTATION
        float rotationDelta = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(1) && rotationDelta != 0) { updateRotation(rotationDelta); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { focus(currentRank + 1); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { focus(currentRank - 1); }
    }

    // =============================
    // ========== PRIVATE ==========
    // =============================
    private void setPivotPosition(Vector3 position) { pivot.transform.position = position; }

    private void setCameraPosition(Vector3 position, bool saveAsDefault=false) {
        if (saveAsDefault)
        {
            cameraSettings.defaultPosition = position;
        }
        cameraGO.transform.position = position;
    }

    private void setCameraRotation(Vector3 rotation, bool saveAsDefault=false) {
        if (saveAsDefault)
        {
            cameraSettings.defaultRotation = rotation;
        }
        cameraGO.transform.eulerAngles = rotation;
    }

    private void setCameraZoom(float zoom, bool saveAsDefault=false) {
        if (saveAsDefault)
        {
            cameraSettings.defaultSize = zoom;
        }
        camera.orthographicSize = zoom;
    }

    private void activateGalleryModel(int rank)
    {
        GameObject go = galleryManager.galleryModels[rank].GalleryModelGO;
        go.transform.Find("Info").gameObject.SetActive(true); // Activate all panels
    }

    private void deactivateGalleryModel(int rank)
    {
        GameObject go = galleryManager.galleryModels[rank].GalleryModelGO;
        go.transform.Find("Info").gameObject.SetActive(false); // Deactivate all panels
    }

    private void focus(int rank)
    {
        if(currentRank != -1) { deactivateGalleryModel(currentRank); }

        currentRank = Utility.Mod(rank, maxRank);

        GalleryModelOrder   galleryModelOrder   = galleryManager.galleryModels[currentRank];
        Transform           cameraSpot          = galleryModelOrder.GalleryModelGO.GetComponent<GalleryModel>().cameraSpot;

        setPivotPosition(galleryModelOrder.GalleryModelGO.transform.position);
        setCameraPosition(cameraSpot.position);
        setCameraRotation(cameraSpot.eulerAngles);
        activateGalleryModel(currentRank);
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

    private GalleryModel getNextGalleryModel(int actualRank)
    {
        GalleryModel galleryModel = null;
        return galleryModel;
    }
}