using UnityEngine;
using UnityEngine.UI;


public class DifficultyLevel : MonoBehaviour {
    public string           name;
    [TextArea(3,10)]
    public string           description;
    public Object           data;                       // Data containing information for generating a level
    public bool             victory;                    // Used to unlocked children
    public Text             Title;                      // UI STUFF
    public Text             Descrition;                      // UI STUFF
}