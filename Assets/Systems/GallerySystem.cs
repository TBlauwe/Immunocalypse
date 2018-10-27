using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

using FYFY;

public class GallerySystem : FSystem {

    // =============================
    // ========== MEMBERS ==========
    // =============================

    private Family _modelsGO = FamilyManager.getFamily(new AllOfComponents(typeof(GalleryModel)));

    public GallerySystem()
    {
        foreach(GameObject go in _modelsGO)
        {
            setupGalleryModel(go);
        }
    }

    // ===========================
    // ========== LOOPS ==========
    // ===========================

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach(GameObject go in _modelsGO)
        {
            if (hasStateChanged(go)) { refresh(go); }
        }
	}

    // ===========================
    // ========== SETUP ==========
    // ===========================
    
    private void refresh(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();

        if (galleryModel.isUnlocked) { unlockModel(go); }
        else { lockModel(go); }

        if (galleryModel.isHighlighted) { highlight(go); }
        else { dehighlight(go); }

        if (galleryModel.isFocused) { focus(go); }
        else { unfocus(go); }

        refreshLight(go);
    }

    private bool hasStateChanged(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();

        if (galleryModel.cacheIsUnlocked != galleryModel.isUnlocked) { return true; }

        if (galleryModel.cacheIsHighlighted != galleryModel.isHighlighted) { return true; }

        if (galleryModel.cacheIsFocused != galleryModel.isFocused) { return true; }

        return false;
    }

    private void setupGalleryModel(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();

        setModelName(go, galleryModel.modelName);
        setScientificName(go, galleryModel.scientificName);
        setSize(go, galleryModel.size);
        setDescription(go, galleryModel.description);
        setClip(go, galleryModel.videoClip);

        galleryModel.cameraSpot = go.transform.Find("CameraSpot").gameObject.transform;

        refresh(go);
    }

    // =============================
    // ========== PRIVATE ==========
    // =============================

    private void unlockModel(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();
        galleryModel.isUnlocked = true;
        galleryModel.cacheIsUnlocked = true;
    }

    private void lockModel(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();
        galleryModel.isUnlocked = false;
        galleryModel.cacheIsUnlocked = false;
    }

    private void focus(GameObject go)
    {
        highlight(go);

        GalleryModel galleryModel = go.GetComponent<GalleryModel>();
        galleryModel.isFocused = true;
        galleryModel.cacheIsFocused = true;

        togglePanel(go);
    }

    private void unfocus(GameObject go)
    {
        dehighlight(go);

        GalleryModel galleryModel = go.GetComponent<GalleryModel>();
        galleryModel.isFocused = false;
        galleryModel.cacheIsFocused = false;

        togglePanel(go, false);
    }

    private void highlight(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();
        galleryModel.isHighlighted = true;
        galleryModel.cacheIsHighlighted = true;
    }

    private void dehighlight(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();
        galleryModel.isHighlighted = false;
        galleryModel.cacheIsHighlighted = false;
    }

    private void togglePanel(GameObject go, bool state=true)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();

        go.transform.Find("Info").gameObject.SetActive(state);

        if (galleryModel.isUnlocked){
            go.transform.Find("Info/Panel").gameObject.SetActive(true);
            go.transform.Find("Info/Lock").gameObject.SetActive(false);
        }
        else {
            go.transform.Find("Info/Panel").gameObject.SetActive(false);
            go.transform.Find("Info/Lock").gameObject.SetActive(true);
        }
    }

    private void refreshLight(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();

        if ((galleryModel.isFocused && galleryModel.isUnlocked) || galleryModel.isHighlighted) {
            go.transform.Find("Light").gameObject.GetComponent<Light>().enabled = true;
                go.transform.Find("ScaryLight").gameObject.GetComponent<Light>().enabled = false;
        }
        else {
            if (galleryModel.isFocused) {
                go.transform.Find("Light").gameObject.GetComponent<Light>().enabled = false;
                go.transform.Find("ScaryLight").gameObject.GetComponent<Light>().enabled = true;
            }
            else
            {
                go.transform.Find("ScaryLight").gameObject.GetComponent<Light>().enabled = false;
                go.transform.Find("Light").gameObject.GetComponent<Light>().enabled = false;
            }
        }
    }

        

    // =======================================
    // ========== GETTERS & SETTERS ==========
    // =======================================

    private void setModelName(GameObject go, string value)
    {
        Text text = (Text) go.transform.Find("Info/Panel/ModelName").gameObject.GetComponent<Text>();
        text.text = value;
    }

    private void setScientificName(GameObject go, string value)
    {
        Text text = (Text) go.transform.Find("Info/Panel/ScientificName/Value").gameObject.GetComponent<Text>();
        text.text = value;
    }

    private void setSize(GameObject go, string value)
    {
        Text text = (Text) go.transform.Find("Info/Panel/Size/Value").gameObject.GetComponent<Text>();
        text.text = value;
    }

    private void setDescription(GameObject go, string value)
    {
        Text text = (Text) go.transform.Find("Info/Panel/ScrollView/Viewport/Content/Description").gameObject.GetComponent<Text>();
        text.text = value;
    }

    private void setClip(GameObject go, VideoClip clip)
    {
        VideoPlayer videoPlayer = (VideoPlayer) go.transform.Find("Info/Panel/ScrollView/Viewport/Content/RenderTexture/VideoPlayer").gameObject.GetComponent<VideoPlayer>();
        videoPlayer.clip = clip;
    }
}