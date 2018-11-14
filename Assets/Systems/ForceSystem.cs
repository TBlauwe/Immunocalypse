using UnityEngine;
using FYFY;

public class ForceSystem : FSystem {
    private readonly Family _subToForces = FamilyManager.getFamily(
        new AllOfComponents(typeof(SubjectToForces)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );
    private readonly Family _forceCreators = FamilyManager.getFamily(
        new AllOfComponents(typeof(ForceCreator)), new AllOfProperties(PropertyMatcher.PROPERTY.ACTIVE_SELF)
    );

    protected override void onProcess(int familiesUpdateCount) {
        //foreach (GameObject go in _subToForces)
        //{
        for (int i  = 0; i < _subToForces.Count; ++i) {
            GameObject go = _subToForces.getAt(i);
            SubjectToForces subject = go.GetComponent<SubjectToForces>();
            Vector3 computedVelocity = new Vector3();

            for (int j = 0; j < _forceCreators.Count; ++j) {
                GameObject creator = _forceCreators.getAt(j);
                ForceCreator[] forces = creator.GetComponents<ForceCreator>();
                foreach (ForceCreator force in forces)
                {
                    int c_mask = force.forceLayerMask;
                    foreach (ForceSpec forceSpec in subject.appliedForces)
                    {
                        //Debug.Log((go.GetComponent<Macrophage>() ? "M" : "P") + " " + forceSpec);
                        if ((c_mask & forceSpec.Mask) > 0)
                        {
                            // We get the distance vector
                            Vector3 r = go.transform.position - creator.transform.position;

                            //We retrieve the distance between them
                            float distance = r.magnitude;

                            Vector3 u = r.normalized;

                            //We compute the force to apply
                            float U = forceSpec.A / Mathf.Pow(distance, forceSpec.N) - forceSpec.B / Mathf.Pow(distance, forceSpec.M);

                            computedVelocity += u * U;
                        }
                    }
                }
            }
            //We normalize the velocity and set it
            computedVelocity.Normalize();
            Debug.DrawRay(go.transform.position, computedVelocity, Color.black);
            go.GetComponent<Rigidbody>().velocity = computedVelocity * subject.speed;
        }
	}
}