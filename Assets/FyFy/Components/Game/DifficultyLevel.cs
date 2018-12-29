using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DifficultyLevel : MonoBehaviour {
    public string           title;                  // Explicit 
    [TextArea(3,10)]
    public string           description;
    public ELevel           scene;                  // ID - Which scene should be loaded
    public EGalleryModel    galleryModelReward;     // Unlock this model in the gallery

    public Text             Title;                  // UI STUFF
    public Text             Descrition;             // UI STUFF
}