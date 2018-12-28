using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DifficultyLevel : MonoBehaviour {
    public string               title;                  // ID
    [TextArea(3,10)]
    public string               description;

    public bool                 selected=false;         // Explicit

    public List<FactoryEntry>   factories;              // Data containing information for generating a level
    public string               scene;                  // Which scene should be loaded

    public bool                 victory;                // Used to unlocked children

    public Text             Title;                      // UI STUFF
    public Text             Descrition;                 // UI STUFF
    public Button           PlayButton;                 // UI STUFF
}