using UnityEngine;
using System.Collections.Generic;

public class LevelButton : MonoBehaviour {
    public string                   title="PlaceHolder";        // Explicit - Used to fill UI panel - ID
    [TextArea(3,10)]
    public string                   description;                // Level description used to inform the user about what he will face

    public bool                     selected=false;             // Explicit

    public GameObject               difficultyLevels;           // Ref to owned difficulty level GOs
}