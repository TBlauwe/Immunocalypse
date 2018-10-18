using UnityEngine;
using FYFY;

public class ForceSystem : FSystem {
    private readonly Family _subToForces = FamilyManager.getFamily(new AllOfComponents(typeof(SubjectToForces)));
    private readonly Family _forceCreators = FamilyManager.getFamily(new AllOfComponents(typeof(ForceCreator)));


    protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _subToForces)
        {
            SubjectToForces subject = go.GetComponent<SubjectToForces>();
            Vector3 computedVelocity = new Vector3();

            foreach (GameObject creator in _forceCreators)
            {
                int c_mask = creator.GetComponent<ForceCreator>().forceLayerMask;
                foreach (ForceSpec forceSpec in subject.appliedForces)
                {
                    if ((c_mask & forceSpec.Mask) > 0)
                    {
                        //We make sure to avoid the same object
                        if (go.Equals(creator)) continue;

                        // We get the distance vector
                        Vector3 r = (go.transform.position - creator.transform.position);

                        //We retrieve the distance between them
                        float distance = r.magnitude;

                        Vector3 u = r.normalized;
                        float a = 0.1f;
                        float b = 0.0f;
                        float n = 2f;
                        float m = 2f;

                        //We compute the force to apply
                        float U = a / Mathf.Pow(distance, n) - b / Mathf.Pow(distance, m);

                        computedVelocity += u * U;
                    }
                }
            }
            //We normalize the velocity and set it
            computedVelocity.Normalize();
            go.GetComponent<Rigidbody>().velocity = computedVelocity;
        }
	}
}