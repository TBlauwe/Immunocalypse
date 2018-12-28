using UnityEngine;
using FYFY;

public class MainMenuSystem : FSystem {
    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family singletonManager = FamilyManager.getFamily(new AllOfComponents(typeof(MainMenuManager)));

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public MainMenuSystem()
    {
        MainMenuManager manager = singletonManager.First().GetComponent<MainMenuManager>();

        if (!manager)
        {
            Debug.LogError("No manager in Scene");
            return;
        }
        manager.Play.onClick.AddListener(Play);
        manager.Gallery.onClick.AddListener(Gallery);
        manager.Quit.onClick.AddListener(Quit);
    }

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
    public void Play()
    {
        GameObjectManager.loadScene("LevelSelectionScene");
    }

    public void Gallery()
    {
        GameObjectManager.loadScene("GalleryScene");
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
             Application.Quit();
        #endif
    }
}