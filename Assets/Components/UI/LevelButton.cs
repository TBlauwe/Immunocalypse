using UnityEngine;
using System.Collections.Generic;

public class LevelButton : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
    public string           levelName="PlaceHolder";   // Explicit - Used to fill UI panel
    public Object           data;                      // Data containing information for generating a level
    [TextArea(3,10)]
    public string           description;               // Level description used to inform the user about what he will face
    public List<GameObject> parents;                   // Is clickable only if parents have been won;
    public bool             victory;                   // Used to unlocked children
    [HideInInspector]
    public bool             victoryCached;             // Used to detect changes

    public Color            victoryNormalColor;        // If victory true, contains normal color (used to switch between different colors)
    public Color            victoryHighlightedColor;   // If victory trus, contains normal color (used to switch between different colors)

    public Color            normalColor;        // If victory true, contains normal color (used to switch between different colors)
    public Color            highlightedColor;   // If victory trus, contains normal color (used to switch between different colors)

}