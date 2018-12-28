using UnityEngine;
using FYFY;

public class PersistentDataSystem : FSystem {

    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family singletonPersistentData = FamilyManager.getFamily(new AllOfComponents(typeof(PersistentData)));

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
        PersistentData data = go.AddComponent<PersistentData>();
        go.AddComponent<DontDestroyOnLoad>();

        GameObjectManager.bind(go);
        Global.data = data;
    }
}