using UnityEngine;
using FYFY;

public class UIStatSystem : FSystem {

    private readonly Family _stats = FamilyManager.getFamily(new AllOfComponents(typeof(UI_Stat)));

    protected override void onProcess(int familiesUpdateCount)
    {
        foreach (GameObject go in _stats)
        {
            UpdateStat(go);
        }
    }

    private void UpdateStat(GameObject go)
    {
        UI_Stat stat = go.GetComponent<UI_Stat>();

        // Initialize card if not done yet
        stat.Name.text = stat.nameText;
        stat.Actual.text = stat.actualText;
        stat.Target.text = stat.targetText;

    }
}