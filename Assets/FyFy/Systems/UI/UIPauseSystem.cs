using UnityEngine;
using FYFY;

public class UIPauseSystem : FSystem {

    // =============================
    // ========== MEMBERS ==========
    // =============================

    private Family _UIPauseGO = FamilyManager.getFamily(new AllOfComponents(typeof(UI_ShowOnPause)));

    private bool active = false;

    // ================================
    // ========== MAIN LOOPS ==========
    // ================================

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        if (Input.GetKeyDown(KeyCode.Escape)) { toggleState(); }
	}

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================

    private void toggleState()
    {
        active = !active;
        foreach (GameObject go in _UIPauseGO)
        {
            processUIElement(go);
        }
    }
    private void processUIElement(GameObject go)
    {
        UI_ShowOnPause comp = go.GetComponent<UI_ShowOnPause>();
        go.SetActive(active == comp.ShowOnPause);
    }

}