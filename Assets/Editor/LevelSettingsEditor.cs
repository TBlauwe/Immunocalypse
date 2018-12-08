using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(LevelSettings))]
public class LevelSettingsEditor : Editor {

    private LevelSettings level;          // Reference to the level

    private bool showFactoryList = true;    // Bool to track if Factories prefabs' foldout must be expanded or collapsed

    private string exportedFileName = "level";

    private readonly string levelsFolder =  "/Resources/levels/";

    private void OnEnable()
    {
        level = (LevelSettings) target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Editor for generating a level for the current scene", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Settings", EditorStyles.boldLabel);
            level.radius    = EditorGUILayout.IntSlider(new GUIContent("Radius", "Radius of hexagonal grids reserved for cells"), level.radius, 1, 100);
            level.cellSizeX = EditorGUILayout.Slider(new GUIContent("Size X", "Cell's X size "), level.cellSizeX, 0.1f, 100.0f);
            level.cellSizeZ = EditorGUILayout.Slider(new GUIContent("Size Z", "Cell's Z size"), level.cellSizeZ, 0.1f, 100.0f);
            level.cellPrefab = EditorGUILayout.ObjectField(new GUIContent("Cell Prefab", "GameObject used to represent a cell"), level.cellPrefab, typeof(GameObject), true) as GameObject;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Arena Settings", EditorStyles.boldLabel);
            level.arenaType = (ARENA_TYPE)EditorGUILayout.EnumPopup(new GUIContent("Arena Type", "Petri - Use a petri's box as arena"), level.arenaType);
            level.arenaPrefab = EditorGUILayout.ObjectField(new GUIContent("Arena Prefab", "GameObject used to represent the arena"), level.arenaPrefab, typeof(GameObject), true) as GameObject;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Generation", EditorStyles.boldLabel);
            level.gridType = (GRID_TYPE)EditorGUILayout.EnumPopup(new GUIContent("Type", "HEXAGON - Generate three rings : Factories, SafeZone, Cells space (Outer to inner)"), level.gridType);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        switch (level.gridType)
        {
            case GRID_TYPE.HEXAGON:
                hexGUI();
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Factories List " + (level.factoryPrefabList.Count == 0 ? "(no child)" : "(" + level.factoryPrefabList.Count.ToString() + " children)"), EditorStyles.boldLabel);
        showFactoryList = GUILayout.Toggle(showFactoryList, "Click to " + (showFactoryList ? "collapse" : "expand"), "Foldout", GUILayout.ExpandWidth(false));
            if (showFactoryList)
            {
                for(int i=0; i<level.factoryPrefabList.Count; i++)
                {
                    drawFactoryEntry(i);
                }

                EditorGUILayout.BeginHorizontal();
                    addFactoryButton();
                    clearFactoryButton();
                EditorGUILayout.EndHorizontal();
            }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Export to file", EditorStyles.boldLabel);
        exportedFileName = EditorGUILayout.TextField(exportedFileName);
        if(exportedFileName != "" && GUILayout.Button("save"))
        {
            string filePath = Application.dataPath + levelsFolder + exportedFileName + ".json";
            File.WriteAllText(filePath, JsonUtility.ToJson(level));
            Debug.Log("Exported level to : " + filePath);
        }
        // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); // Splitter for graphical's purposes
    }

    // ======================================   
    // ========== Editor's Button  ==========   
    // ======================================   

    private void addFactoryButton()
    {
        if (GUILayout.Button("Add")){
            level.factoryPrefabList.Add(null);
            level.factoryNumberList.Add(0);
        }
    }

    private void clearFactoryButton()
    {
        if (GUILayout.Button("Clear")){
            level.factoryPrefabList.Clear();
            level.factoryNumberList.Clear();
        }
    }

    private void removeFactoryButton(int index)
    {
        if (GUILayout.Button("X")){
            level.factoryPrefabList.RemoveAt(index);
            level.factoryNumberList.RemoveAt(index);
        }
    }

    // ===============================
    // ========== Painters  ==========   
    // ===============================

    private void drawFactoryEntry(int index)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Factory " + (index + 1).ToString());
            level.factoryPrefabList[index] = EditorGUILayout.ObjectField(level.factoryPrefabList[index], typeof(GameObject), true) as GameObject;
            EditorGUILayout.LabelField("Number",  GUILayout.MaxWidth(55.0f));
            level.factoryNumberList[index] = EditorGUILayout.IntField(level.factoryNumberList[index], GUILayout.MaxWidth(25.0f));
            removeFactoryButton(index);
        EditorGUILayout.EndHorizontal();
    }

    private void hexGUI()
    {
        int totalHex = Hex.CountHexesInIsland(level.radius);
        int maxIslandSize = totalHex / level.islandsNumber;
        int maxIslandNumber = totalHex / Hex.CountHexesInIsland(level.islandMaxSize);
        EditorGUILayout.LabelField("Layers settings", EditorStyles.boldLabel);
            level.numberOfFactoriesLayers = EditorGUILayout.IntSlider(new GUIContent("Factories layers", "Number of layers for the factories"), level.numberOfFactoriesLayers, 1, 100);
            level.numberOffSafeZoneLayers = EditorGUILayout.IntSlider(new GUIContent("Safe zone layers", "Number of layers between factories' rings and cells' rings"), level.numberOffSafeZoneLayers, 1, 100);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Islands Settings", EditorStyles.boldLabel);
            level.islandMinSize = EditorGUILayout.IntSlider(new GUIContent("Island Min Size", "Explicit"), level.islandMinSize, 1, maxIslandSize);
            level.islandMaxSize = EditorGUILayout.IntSlider(new GUIContent("Island Max Size", "Explicit"), level.islandMaxSize, level.islandMinSize, maxIslandSize);
            level.islandsNumber = EditorGUILayout.IntSlider(new GUIContent("Number of Islands", "Explicit"), level.islandsNumber, 1, maxIslandNumber);
    }

}
