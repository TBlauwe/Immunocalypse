using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGeneration))]
public class LevelGenerationEditor : Editor {

    private LevelGeneration level;          // Reference to the level

    private bool showFactoryList = true;    // Bool to track if foldout must be expanded or collapsed

    private void OnEnable()
    {
        level = (LevelGeneration) target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Editor for generating a level for the current scene", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Settings", EditorStyles.boldLabel);
            level.radius = EditorGUILayout.IntSlider(new GUIContent("Radius", "Radius of hexagonal grids reserved for cells"), level.radius, 1, 100);
            level.cellPrefab = EditorGUILayout.ObjectField(new GUIContent("Cell Prefab", "GameObject used to represent a cell"), level.cellPrefab, typeof(GameObject), true) as GameObject;
            level.petriPrefab = EditorGUILayout.ObjectField(new GUIContent("Petri Prefab", "GameObject used to represent the arena"), level.petriPrefab, typeof(GameObject), true) as GameObject;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Generation", EditorStyles.boldLabel);
            level.gen = (GENERATEUR)EditorGUILayout.EnumPopup(new GUIContent("Type", "HEXAGON - Generate three rings : Factories, SafeZone, Cells space (Outer to inner)"), level.gen);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        switch (level.gen)
        {
            case GENERATEUR.HEXAGON:
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

        // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); // Splitter

    }

    // ======================================   
    // ========== Editor's Button  ==========   
    // ======================================   

    private void addFactoryButton()
    {
        if (GUILayout.Button("Add")){
            level.factoryPrefabList.Add(new GameObject());
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
        int totalHex = Mathf.FloorToInt(Mathf.Pow(level.radius, 3) - Mathf.Pow((level.radius - 1), 3));
        int maxIslandSize = totalHex / level.islandsNumber;
        int maxIslandNumber = totalHex / level.islandMaxSize;
        EditorGUILayout.LabelField("Rings settings", EditorStyles.boldLabel);
            level.reservedFactoryRing = EditorGUILayout.IntSlider(new GUIContent("Factories rings", "Rings reserved for the factories starting at the outer rings"), level.reservedFactoryRing, 1, 100);
            level.safeZoneRings = EditorGUILayout.IntSlider(new GUIContent("Safe zone rings", "No cell's land between factories' rings and cells' rings"), level.safeZoneRings, 1, 100);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Islands Settings", EditorStyles.boldLabel);
            level.islandMinSize = EditorGUILayout.IntSlider(new GUIContent("Island Min Size", "Explicit"), level.islandMinSize, 1, maxIslandSize);
            level.islandMaxSize = EditorGUILayout.IntSlider(new GUIContent("Island Max Size", "Explicit"), level.islandMaxSize, level.islandMinSize, maxIslandSize);
            level.islandsNumber = EditorGUILayout.IntSlider(new GUIContent("Number of Islands", "Explicit"), level.islandsNumber, 1, maxIslandNumber);
    }
}
