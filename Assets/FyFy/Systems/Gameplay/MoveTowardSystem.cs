using UnityEngine;
using FYFY;

public class MoveTowardSystem : FSystem {
    private Family _MoveTowardGO = FamilyManager.getFamily(
        new AllOfComponents(typeof(MoveToward)),
        new NoneOfComponents(typeof(Frozen))
    );

	protected override void onProcess(int familiesUpdateCount) {
        int count = _MoveTowardGO.Count;
        for (int i=0; i<count; i++){
            GameObject go = _MoveTowardGO.getAt(i);
            MoveToward moveToward = go.GetComponent<MoveToward>();
            float step = moveToward.speed * Time.deltaTime;
            go.transform.position = Vector3.MoveTowards(go.transform.position, moveToward.target, step);
        }
	}
}