using UnityEngine;
using FYFY;

public class MovingSystem : FSystem {
    private readonly Family _movables = FamilyManager.getFamily(new AllOfComponents(
        typeof(Movable), typeof(Rigidbody)
    ));
    private readonly Family _pathogenes = FamilyManager.getFamily(new AnyOfTags("pathogene"));
    private readonly Family _defenses = FamilyManager.getFamily(new AnyOfTags("defense_site"));

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
        foreach (GameObject go in _movables)
        {
            Movable movable = go.GetComponent<Movable>();
            Rigidbody rbody = go.GetComponent<Rigidbody>();

            Vector3 velocity = new Vector3();
            foreach (GameObject pathogene in _pathogenes)
            {
                if (!pathogene.Equals(go))
                {
                    Vector3 r = go.transform.position - pathogene.transform.position;
                    Vector3 u = new Vector3(r.x, r.y, r.z);
                    u.Normalize();

                    float a = .6f;
                    float b = .1f;
                    float n = 2;
                    float m = 1f;
                    float d = r.magnitude;

                    float U = a / Mathf.Pow(d, n) - b / Mathf.Pow(d, m);

                    velocity += U * u;
                }
            }
            foreach (GameObject target in _defenses)
            {
                Vector3 r = go.transform.position - target.transform.position;
                Vector3 u = new Vector3(r.x, r.y, r.z);
                u.Normalize();

                float a = .4f;
                float b = .4f;
                float n = 2f;
                float m = 1f;
                float d = r.magnitude;

                float U = a / Mathf.Pow(d, n) - b / Mathf.Pow(d, m);

                velocity += U * u;
            }
            velocity.Normalize();
            velocity = new Vector3(
                Mathf.Round(velocity.x),
                Mathf.Round(velocity.y),
                Mathf.Round(velocity.z)
            );
            rbody.velocity = movable.velocity * velocity;
        }
	}
}