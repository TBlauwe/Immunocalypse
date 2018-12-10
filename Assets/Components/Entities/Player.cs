using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // The player's global deck
    public List<GameObject> globalDeck = new List<GameObject>();

    // The player's level deck
    public List<GameObject> levelDeck = new List<GameObject>();

    // The player's selected level
    public LevelButton levelButton;
}