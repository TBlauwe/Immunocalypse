﻿using UnityEngine;

public class Damager : MonoBehaviour {
    // Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public float damagesPerSec;
    [HideInInspector] public float cooldown;
}