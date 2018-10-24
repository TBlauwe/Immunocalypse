using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

using FYFY;

public class GallerySystem : FSystem {

    private Family _modelsGO = FamilyManager.getFamily(new AllOfComponents(typeof(GalleryModel)));

    public GallerySystem()
    {
        foreach(GameObject go in _modelsGO)
        {
            setupGalleryModel(go);
        }
    }

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        foreach(GameObject go in _modelsGO)
        {
            GalleryModel galleryModel = go.GetComponent<GalleryModel>();

            if (galleryModel.unlocked) { unlockModel(go); }
            else { lockModel(go); }
        }
	}

    private void unlockModel(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();

        Light spotlight = (Light) go.transform.Find("Light").gameObject.GetComponent<Light>();
        spotlight.enabled = true;

        GameObject panel = go.transform.Find("Info/Panel").gameObject;
        panel.SetActive(true);

        GameObject lockPanel = go.transform.Find("Info/Lock").gameObject;
        lockPanel.SetActive(false);


        galleryModel.unlocked = true;
        galleryModel.cacheUnlocked = true;
    }

    private void lockModel(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();

        GameObject panel = go.transform.Find("Info/Panel").gameObject;
        panel.SetActive(false);

        GameObject lockPanel = go.transform.Find("Info/Lock").gameObject;
        lockPanel.SetActive(true);

        Light spotlight = (Light) go.transform.Find("Light").gameObject.GetComponent<Light>();
        spotlight.enabled = false; 

        galleryModel.unlocked = false;
        galleryModel.cacheUnlocked = false;
    }

    private void setupGalleryModel(GameObject go)
    {
        GalleryModel galleryModel = go.GetComponent<GalleryModel>();

        setModelName(go, galleryModel.modelName);
        setScientificName(go, galleryModel.scientificName);
        setSize(go, galleryModel.size);
        setDescription(go, galleryModel.description);
        setClip(go, galleryModel.videoClip);

        if (galleryModel.unlocked != galleryModel.cacheUnlocked)
        {
            if (galleryModel.unlocked) { unlockModel(go); }
            else { lockModel(go); }
        }
    }

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