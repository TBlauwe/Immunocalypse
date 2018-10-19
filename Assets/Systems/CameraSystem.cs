using UnityEngine;
using FYFY;

public class CameraSystem : FSystem {

    private Family _cameraGO = FamilyManager.getFamily(new AllOfComponents(typeof(Camera), typeof(CameraControl)));
    private Family _levelGO = FamilyManager.getFamily(new AllOfComponents(typeof(LevelGeneration)));

    public CameraSystem()
    {
        setupCamera(_cameraGO.First(), _levelGO.First());
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

        updateCamera(_cameraGO.First());
	}

    private void setupCamera(GameObject go, GameObject level)
    {
        Camera camera = go.GetComponent<Camera>();
        CameraControl cameraControl = go.GetComponent<CameraControl>();

        // --------------------
        // --- SET POSITION ---
        // --------------------
        Vector3 size = level.GetComponent<LevelGeneration>().petriBox.transform.lossyScale / 2;

        // Y Offset to center the grid in the view
        float xDistance = size.x * cameraControl.offset;
        float yDistance = Vector3.Distance(Vector3.zero, Vector3.right * xDistance);

        // XZ Offset to center the grid in the view  
        // float xzDistance = Vector3.Distance(Vector3.zero, size);

        // go.transform.position = grid.GetComponent<HexGrid>().getSize() / 2 + Vector3.up * yDistance;
        // Vector3 normal = Vector3.Cross(new Vector3(size.x, go.transform.position.y, -size.z),
        //                                new Vector3(-size.x, go.transform.position.y, size.z)).normalized * -1;

        // go.transform.position = normal * xzDistance + Vector3.up * yDistance;
        go.transform.position = Vector3.right * xDistance + Vector3.up * (yDistance);


        // --------------------
        // --- SET ROTATION ---
        // --------------------
        // float angle = Vector3.Angle(go.transform.forward, Vector3.zero - go.transform.position);
        // camera.transform.RotateAround(camera.transform.position, Vector3.up, angle);

        camera.transform.eulerAngles = new Vector3(45, -90);

        // --------------------
        // --- SET DEFAULTS ---
        // --------------------
        
        camera.orthographicSize = size.x + 5.0f;

        cameraControl.defaultSize = camera.orthographicSize;
        cameraControl.defaultPosition = go.transform.position;
        cameraControl.defaultRotation = go.transform.rotation;
    }

    private void updateCamera(GameObject go)
    {
        Camera camera = go.GetComponent<Camera>();
        CameraControl cameraControl = go.GetComponent<CameraControl>();

        // Update translation
        var translation = GetInputTranslationDirection() * cameraControl.MoveSpeed * Time.deltaTime;
        Vector3 rotatedTranslation = Quaternion.Euler(  camera.transform.eulerAngles.x,
                                                        camera.transform.eulerAngles.y,
                                                        camera.transform.eulerAngles.z) * translation;

        camera.transform.position = smoothTranslation(  camera.transform.position,
                                                        camera.transform.position + rotatedTranslation,
                                                        cameraControl.MoveSpeed);

        // Update Zoom
        camera.orthographicSize = Mathf.Clamp(  camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * cameraControl.ZoomSpeed,
                                                cameraControl.ZoomMin,
                                                cameraControl.ZoomMax);

        // Update Rotation
        if (Input.GetMouseButton(1))
        {
            Vector3 rotation = new Vector3(0, Input.GetAxis("Mouse X"), 0);
            go.transform.parent.Rotate(rotation * Time.deltaTime * cameraControl.RotationSpeed);
        }

        // Reset position
        if (Input.GetKey(KeyCode.R))
        {
            go.transform.position = smoothTranslation(go.transform.position, cameraControl.defaultPosition, cameraControl.MoveSpeed / 100); 
            go.transform.rotation = smoothRotation(go.transform.rotation, cameraControl.defaultRotation, cameraControl.RotationSpeed / 50);
            camera.orthographicSize = smoothZoom(camera.orthographicSize, cameraControl.defaultSize, cameraControl.ZoomSpeed / 10);
        }
    }

    private float smoothZoom(float from, float to, float speed)
    {
        return Mathf.SmoothStep(from, to, Time.deltaTime * speed);
    }

    private Quaternion smoothRotation(Quaternion from, Quaternion to, float speed)
    {
        return Quaternion.Slerp(from, to, Time.deltaTime * speed);
    }

    private Vector3 smoothTranslation(Vector3 from, Vector3 to, float speed)
    {
        return Vector3.Slerp(from, to, Time.deltaTime * speed);
    }

    private Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
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