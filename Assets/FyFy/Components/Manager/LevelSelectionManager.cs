using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour {
    public GameObject       menuLevelSelection;           
    public GameObject       menuDifficultySelection;           
    public Button           buttonNextMenu;
    public Button           buttonPrevMenu;

    public bool             isOnMenuDifficultySelection = false; 
    public KeyCode          returnKey = KeyCode.Backspace;
}