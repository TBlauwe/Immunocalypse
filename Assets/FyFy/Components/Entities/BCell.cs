using FYFY_plugins.PointerManager;
using UnityEngine;

[DisallowMultipleComponent]
public class BCell : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public int freezeMask;
    public float delay;
    [HideInInspector] public float cooldown;
    public float freezeTime;
    public ParticleSystem onClickParticleEffect;
}