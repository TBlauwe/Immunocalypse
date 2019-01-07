using UnityEngine;
using FYFY;
using System;

public class PersistentDataSystem : FSystem {

    // =============================
    // ========== MEMBERS ==========
    // =============================
    private readonly Family singletonPersistentData = FamilyManager.getFamily(new AllOfComponents(typeof(PersistentData)));
    private readonly Family singletonPlayer = FamilyManager.getFamily(new AllOfComponents(typeof(Player)));
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

        YellowPageComponent yp = go.AddComponent<YellowPageComponent>();
        yp.items = new YellowPageItem[] { null, null, null};

        YellowPageItem item = YellowPageUtils.AddItem(yp);
        item.key = "Macrophage";
        item.sourceObject = Resources.Load<GameObject>("Prefabs/Units/Macrophage");

        item = YellowPageUtils.AddItem(yp);
        item.key = "E.Coli";
        item.sourceObject = Resources.Load<GameObject>("Prefabs/Units/E.Coli");

        item = YellowPageUtils.AddItem(yp);
        item.key = "BCell[E.Coli]";
        item.sourceObject = Resources.Load<GameObject>("Prefabs/Units/BCell[E.Coli]");

        GameObjectManager.bind(go);
        Global.data = persistentData;
        Global.player = singletonPlayer.First().GetComponent<Player>();
    }
}