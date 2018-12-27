using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class LevelSelectorSystem : FSystem {

    // =============================
    // ========== MEMBERS ==========
    // =============================
    private Family _levelGO = FamilyManager.getFamily(new AllOfComponents(typeof(DifficultyLevel)));

    // ======================================
    // ========== PUBLIC FUNCTIONS ==========
    // ======================================
    public LevelSelectorSystem()
    {
        GameObject container = GameObject.Find("Container");
        foreach(GameObject go in PersistentData.Level.selectedLevel.GetComponent<LevelButton>().difficultyLevels)
        {
            GameObjectManager.bind(go);
            setupDifficultyLevel(go);
            go.transform.parent = container.transform;
        }
    }

    // ===========================
    // ========== LOOPS ==========
    // ===========================
    protected override void onProcess(int familiesUpdateCount)
    {
        /**
        foreach(GameObject go in _levelGO)
        {
        }
        **/
    }

    // =======================================
    // ========== PRIVATE FUNCTIONS ==========
    // =======================================
    private void setupDifficultyLevel(GameObject go)
    {
        DifficultyLevel difficultyLevel = go.GetComponent<DifficultyLevel>();
        difficultyLevel.Title.text = difficultyLevel.name;
        difficultyLevel.Descrition.text = difficultyLevel.description;
    }
}