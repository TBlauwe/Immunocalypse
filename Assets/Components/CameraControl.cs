using UnityEngine;

public class CameraControl : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

    public float ZoomSpeed = 10.0f;
    public float ZoomMin = 0.5f;
    public float ZoomMax = 100;

    public float MoveSpeed = 10.0f;                      
    public float RotationSpeed = 15.0f;                  

    public float offset = 10.0f;                      

    [HideInInspector]
    public Vector3 defaultPosition;

    [HideInInspector]
    public Quaternion defaultRotation;

    [HideInInspector]
    public float defaultSize;
}