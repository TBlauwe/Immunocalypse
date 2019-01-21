using UnityEngine;
using System.Collections.Generic;

public class StartLoopTrigger : MonoBehaviour {
    public List<GameObject> deckPool;
    public List<GameObject> randomGoPool;

    public float deckRate;
    public float randomGoRate;

    [HideInInspector] public float cooldownDeck;
    [HideInInspector] public float cooldownRandom;
}