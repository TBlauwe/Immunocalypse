using UnityEngine;
using FYFY;
using System;

public class PersistentDataSystem : FSystem {

    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family singletonPersistentData = FamilyManager.getFamily(new AllOfComponents(typeof(PersistentData)));
    private PersistentData persistentData;

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public PersistentDataSystem()
    {
        if(singletonPersistentData.First() == null)
            initialization();

    }

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
    private void initialization()
    {
        // Create GO containing persistentData 
        GameObject go = new GameObject("PersistentData");

        persistentData = go.AddComponent<PersistentData>();
        foreach(ELevel level in Enum.GetValues(typeof(ELevel)))
        {
            persistentData.succeededLevels.Add(new PairELevelBool(level, false));
        }

        foreach(EGalleryModel galleryModel in Enum.GetValues(typeof(EGalleryModel)))
        {
            persistentData.unlockedGalleryModels.Add(new PairEGalleryModelBool(galleryModel, false));
        }

        go.AddComponent<DontDestroyOnLoad>();

        GameObjectManager.bind(go);
        Global.data = persistentData;
    }
}