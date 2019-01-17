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

        foreach(EStatTrackedEntity trackedEntity in Enum.GetValues(typeof(EStatTrackedEntity)))
        {
            persistentData.trackedEntities.Add(new PairEStatTrackedEntityInt(trackedEntity, 0));
        }

        go.AddComponent<DontDestroyOnLoad>();

        YellowPageComponent yp = go.AddComponent<YellowPageComponent>();
        yp.items = new YellowPageItem[5]; // { null, null, null, null, null};

        YellowPageItem item = YellowPageUtils.AddItem(yp);
        item.key = "Macrophage";
        item.sourceObject = Resources.Load<GameObject>("Prefabs/Units/Macrophage");

        item = YellowPageUtils.AddItem(yp);
        item.key = "E.coli";
        item.sourceObject = Resources.Load<GameObject>("Prefabs/Units/E.coli");

        item = YellowPageUtils.AddItem(yp);
        item.key = "BCell[E.coli]";
        item.sourceObject = Resources.Load<GameObject>("Prefabs/Units/B_Cell[E.coli]");

        item = YellowPageUtils.AddItem(yp);
        item.key = "BCell[Norovirus]";
        item.sourceObject = Resources.Load<GameObject>("Prefabs/Units/B_Cell[Norovirus]");

        item = YellowPageUtils.AddItem(yp);
        item.key = "Norovirus";
        item.sourceObject = Resources.Load<GameObject>("Prefabs/Units/Norovirus");

        GameObjectManager.bind(go);
        Global.data = persistentData;
        Global.player = singletonPlayer.First().GetComponent<Player>();
    }
}