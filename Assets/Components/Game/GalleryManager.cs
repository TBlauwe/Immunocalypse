using UnityEngine;
using System.Collections.Generic;

public enum GALLERY_MODE
{
    CLOSE,
    OVERVIEW
}

public class GalleryManager : MonoBehaviour {
    public GALLERY_MODE             mode;
    public List<GalleryModelOrder>  galleryModels   = new List<GalleryModelOrder>();
    public List<GameObject>         cameraSpots     = new List<GameObject>();
}