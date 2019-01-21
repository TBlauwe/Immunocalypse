using UnityEngine;

public enum CAMERA_MODE
{
    FIXED,
    GAMEPLAY,
    INSPECTION,
    GALLERY
}

public class CameraSettings : MonoBehaviour {
    public CAMERA_MODE mode = CAMERA_MODE.FIXED;

    public float ZoomSpeed      = 100.0f;
    public float ZoomMin        = 0.5f;

    public float MoveSpeed      = 10.0f;                      

    public float RotationSpeed  = 150.0f;

    public float scrollSpeed = 5;

    public float minXScrollingPosition;

    public float maxXScrollingPosition;

    [HideInInspector]
    public float ZoomMax;   // Computed after generation

    [HideInInspector]
    public Vector3 defaultPosition;

    [HideInInspector]
    public Vector3 defaultRotation;

    [HideInInspector]
    public float defaultSize;
}