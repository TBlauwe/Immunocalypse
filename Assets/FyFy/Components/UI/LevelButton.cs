using UnityEngine;
using System.Collections.Generic;

public class LevelButton : MonoBehaviour {
    public string                   title="PlaceHolder";    // Explicit - Used to fill UI panel
    [TextArea(3,10)]
    public string                   description;                // Level description used to inform the user about what he will face
    public GameObject               panel;                      // Ref to UI panel
    public bool                     victory;                    // Used to unlocked children
    public List<GameObject>    difficultyLevels;
    [HideInInspector] public bool   victoryCached;        // Used to detect changes
}