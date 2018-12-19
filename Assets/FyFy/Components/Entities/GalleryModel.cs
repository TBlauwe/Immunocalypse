using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;

[Serializable]
public struct GalleryModelOrder {

	public GameObject GalleryModelGO;
	public int Order;
}

public class GalleryModel : MonoBehaviour {

    // IMPORTANT : Don't change go child structure, 

    public bool         isFocused=false;                // Has the focus ?
    [HideInInspector]
    public bool         cacheIsFocused=false;           // Update only if cache is different from current value

    public bool         isUnlocked=false;                 // Can it be seen ?
    [HideInInspector]
    public bool         cacheIsUnlocked=false;          // Update only if cache is different from current value

    public bool         isHighlighted=false;            // Whether the model should be visible (with lights but without panel)
    [HideInInspector]
    public bool         cacheIsHighlighted=false;       // Update only if cache is different from current value

    public string       modelName="PlaceHolder";        // Explicit - Used to fill UI panel
    public string       scientificName="PlaceHolder";   // Explicit - Used to fill UI panel
    public string       size="PlaceHolder";             // Explicit - Used to fill UI panel

    [TextArea]
    public string       description="PlaceHolder";      // Explicit - Used to fill UI panel

    public VideoClip    videoClip;                      // Clip played at the end of the description - Used to fill UI panel

    [HideInInspector]
    public Transform    cameraSpot;                     // Specify camera location 

}