using UnityEngine;
using FYFY;

public class RotatorSystem : FSystem {
    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family _RotatingGO = FamilyManager.getFamily(new AllOfComponents(typeof(Rotator)));

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public RotatorSystem()
    {
        foreach (GameObject go in _RotatingGO)
        {
            go.transform.eulerAngles += new Vector3(
                go.GetComponent<Rotator>().rotationSpeed.x * Random.Range(0, 360),
                go.GetComponent<Rotator>().rotationSpeed.y * Random.Range(0, 360),
                go.GetComponent<Rotator>().rotationSpeed.z * Random.Range(0, 360));
        }
    }

    // ================================
    // ========== MAIN LOOPS ==========
    // ================================

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _RotatingGO)
        {
            go.transform.eulerAngles += go.GetComponent<Rotator>().rotationSpeed * Time.deltaTime;
        }
	}
}