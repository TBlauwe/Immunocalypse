using UnityEngine;

public class Macrophage : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    /*[HideInInspector]*/ public MacrophageSystem.DECISIONS lastDescision = MacrophageSystem.DECISIONS.FOLLOW_PATH;
}