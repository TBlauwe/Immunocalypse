using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GalleryModel : MonoBehaviour {

    // IMPORTANT : Don't change go child structure, 

    [HideInInspector]
    public bool         focused=false;                  // Has the focus ?
    [HideInInspector]
    public bool         cacheFocused=false;             // Update only if cache is different from current value

    public bool         unlocked=false;                 // Can it be seen ?
    [HideInInspector]
    public bool         cacheUnlocked=false;             // Update only if cache is different from current value

    public string       modelName="PlaceHolder";        // Explicit - Used to fill UI panel
    public string       scientificName="PlaceHolder";   // Explicit - Used to fill UI panel
    public string       size="PlaceHolder";             // Explicit - Used to fill UI panel

    [TextArea]
    public string       description="PlaceHolder";      // Explicit - Used to fill UI panel

    public VideoClip    videoClip;                      // Clip played at the end of the description - Used to fill UI panel

    [HideInInspector]
    public Transform    cameraSpot;                     // Specify camera location 
}