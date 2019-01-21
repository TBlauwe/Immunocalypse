using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FYFY;
using System.Collections;

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
    private GALLERY_MODE    galleryMode;

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
    private void setupCameraForGallery()
    {
        galleryManager = _galleryManagerGO.First().gameObject.GetComponent<GalleryManager>();
        galleryMode = GALLERY_MODE.CLOSE;
        focus(0);
    }


    // ============================
    // ========== UPDATE ==========
    // ============================
    private void updateCameraForGallery()
    {
        float rotationDelta = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(1) && rotationDelta != 0) { updateRotation(rotationDelta); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { focus(currentRank + 1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { focus(currentRank - 1); }
        if (Input.GetKeyDown(KeyCode.Space)) { switchGalleryMode(); }
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

    private void highlightAllGalleryModels()
    {
        foreach(GalleryModelOrder gmOrder in galleryManager.galleryModels)
        {
            gmOrder.GalleryModelGO.GetComponent<GalleryModel>().isHighlighted = true;
            gmOrder.GalleryModelGO.GetComponent<GalleryModel>().isFocused     = false;
        }
    }

    private void dehighlightAllGalleryModels()
    {
        foreach(GalleryModelOrder gmOrder in galleryManager.galleryModels)
        {
            gmOrder.GalleryModelGO.GetComponent<GalleryModel>().isHighlighted   = false;
        }
    }

    private void activateGalleryModel(int rank)
    {
        GalleryModel gm = galleryManager.galleryModels[rank].GalleryModelGO.GetComponent<GalleryModel>();
        gm.isFocused = true;
    }

    private void deactivateGalleryModel(int rank)
    {
        GalleryModel gm = galleryManager.galleryModels[rank].GalleryModelGO.GetComponent<GalleryModel>();
        gm.isFocused = false;
    }

    private void focusOverview(int rank)
    {
        currentRank = Utility.Mod(rank, galleryManager.cameraSpots.Count);

        Transform   cameraSpot  = galleryManager.cameraSpots[currentRank].transform;

        setCameraPosition(cameraSpot.position);
        setCameraRotation(cameraSpot.eulerAngles);
    }

    private void focusClose(int rank)
    {
        if(currentRank != -1) { deactivateGalleryModel(currentRank); }

        currentRank = Utility.Mod(rank, galleryManager.galleryModels.Count);

        GalleryModelOrder   galleryModelOrder   = galleryManager.galleryModels[currentRank];
        Transform           cameraSpot          = galleryModelOrder.GalleryModelGO.GetComponent<GalleryModel>().cameraSpot;

        setPivotPosition(galleryModelOrder.GalleryModelGO.transform.position);
        setCameraPosition(cameraSpot.position);
        setCameraRotation(cameraSpot.eulerAngles);
        activateGalleryModel(currentRank);
    }

    private void focus(int rank)
    {
        switch (galleryMode)
        {
            case GALLERY_MODE.CLOSE:
                focusClose(rank);
                break;

            case GALLERY_MODE.OVERVIEW:
                focusOverview(rank);
                break;
        }
    }

    private void switchGalleryMode()
    {
        if(galleryMode == GALLERY_MODE.CLOSE) {
            galleryMode = GALLERY_MODE.OVERVIEW;
            highlightAllGalleryModels();
        }
        else {
            galleryMode = GALLERY_MODE.CLOSE;
            dehighlightAllGalleryModels();
        }
        currentRank = 0;
        focus(0);
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
}