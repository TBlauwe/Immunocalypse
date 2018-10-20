using UnityEngine;
using FYFY;

public class DynamicMaterialSystem : FSystem {

    private Family _dynGO = FamilyManager.getFamily(new AllOfComponents(typeof(DynamicMaterial)));

    public DynamicMaterialSystem()
    {
        foreach(GameObject go in _dynGO)
        {
            DynamicMaterial dynMat = go.GetComponent<DynamicMaterial>();
            foreach(Material mat in go.GetComponent<Renderer>().materials)
            {
                if (mat.name.Contains(dynMat.materialName) || go.GetComponent<Renderer>().materials.Length == 1)
                {
                    mat.color = dynMat.startingColor;
                }
            }
        }
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
        foreach(GameObject go in _dynGO)
        {
            DynamicMaterial dynMat = go.GetComponent<DynamicMaterial>();
            foreach(Material mat in go.GetComponent<Renderer>().materials)
            {
                if (mat.name.Contains(dynMat.materialName) || go.GetComponent<Renderer>().materials.Length == 1)
                {
                    mat.color = Color.Lerp(dynMat.startingColor, dynMat.endingColor, Mathf.PingPong(Time.time, 1));
                }
            }
        }
	}
}