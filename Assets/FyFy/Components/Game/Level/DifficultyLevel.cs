using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DifficultyLevel : MonoBehaviour {
    public string           title;                  // Explicit 
    [TextArea(3,10)]
    public string           description;
    public ELevel           playScene;              // ID - Which scene should be loaded
    public EInstructions    instructionsScene;      // ID - Which scene should be loaded

    public List<EGalleryModel>  galleryModelRewards;    // Unlock this model in the gallery
    public List<GameObject>     cardRewards;            // Add theses cards in the player's global deck

    public Text             Title;                  // UI STUFF
}