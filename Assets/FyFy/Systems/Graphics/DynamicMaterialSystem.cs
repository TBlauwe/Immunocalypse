using UnityEngine;
using FYFY;

public class DynamicMaterialSystem : FSystem {

    private Family _dynGO = FamilyManager.getFamily(new AllOfComponents(typeof(DynamicMaterial)));

    public DynamicMaterialSystem()
    {
        foreach (GameObject go in _dynGO)
        {
            DynamicMaterial dynMat = go.GetComponent<DynamicMaterial>();

            if (dynMat.type == EDynMat.HEALTH)
            {
                WithHealth healthComp = go.GetComponent<WithHealth>();
                healthComp.maxHealth = healthComp.health;
                dynMat.percentage = healthComp.health / healthComp.maxHealth;

            }
            else if (dynMat.type == EDynMat.FREEZE)
            {
                Frozen comp = go.GetComponent<Frozen>();
                dynMat.percentage = 0;
                if (comp)
                {
                    dynMat.percentage = comp.remainingTime / comp.totalTime;
                }
            }
            else if (dynMat.type == EDynMat.B_Cell)
            {
                BCell comp = go.GetComponent<BCell>();
                if (comp)
                {
                    dynMat.percentage = comp.cooldown / comp.delay;
                }
            }

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

            if(dynMat.type == EDynMat.HEALTH)
            {
                WithHealth healthComp = go.GetComponent<WithHealth>();
                dynMat.percentage = healthComp.health / healthComp.maxHealth;

            }else if(dynMat.type == EDynMat.FREEZE)
            {
                Frozen comp = go.GetComponent<Frozen>();
                dynMat.percentage = 0;
                if (comp)
                {
                    dynMat.percentage = comp.remainingTime / comp.totalTime;
                }
            }
            else if (dynMat.type == EDynMat.B_Cell)
            {
                BCell comp = go.GetComponent<BCell>();
                if (comp)
                {
                    dynMat.percentage = comp.cooldown / comp.delay;
                }
            }

            foreach(Material mat in go.GetComponent<Renderer>().materials)
            {
                if (mat.name.Contains(dynMat.materialName) || go.GetComponent<Renderer>().materials.Length == 1)
                {
                    mat.color = Color.Lerp(dynMat.endingColor, dynMat.startingColor, dynMat.percentage);
                }
            }
        }
	}
}