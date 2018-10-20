using UnityEngine;

public class CameraSettings : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

    public float ZoomSpeed      = 100.0f;
    public float ZoomMin        = 0.5f;

    public float MoveSpeed      = 10.0f;                      

    public float RotationSpeed  = 150.0f;

    [HideInInspector]
    public float ZoomMax;   // Computed after generation

    [HideInInspector]
    public Vector3 defaultPosition;

    [HideInInspector]
    public Vector3 defaultRotation;

    [HideInInspector]
    public float defaultSize;
}