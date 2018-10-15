using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGeneration))]
public class LevelGenerationEditor : Editor {

    private LevelGeneration grid;

    private void OnEnable()
    {
        grid = (LevelGeneration) target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Editor for generating a grid for the current scene", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Settings", EditorStyles.boldLabel);
            grid.radius = EditorGUILayout.IntSlider(new GUIContent("Radius", "Explicit"), grid.radius, 1, 100);
            grid.cellPrefab = EditorGUILayout.ObjectField(new GUIContent("Cell Prefab", "GameObject used to represent a cell"), grid.cellPrefab, typeof(GameObject), true) as GameObject;
            grid.petriPrefab = EditorGUILayout.ObjectField(new GUIContent("Petri Prefab", "GameObject used to represent the arena"), grid.petriPrefab, typeof(GameObject), true) as GameObject;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Generation", EditorStyles.boldLabel);
            grid.gen = (GENERATEUR)EditorGUILayout.EnumPopup(new GUIContent("Type",  "RANDOM - Randomly place cells in grid" + "\n" +
                                                                                "ISLAND - Randomly place island made of cells"), grid.gen);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        switch (grid.gen)
        {
            case GENERATEUR.RANDOM:
                randomGUI();
                break;
            case GENERATEUR.ISLAND:
                islandGUI();
                break;
        }
    }

    private void randomGUI()
    {
        EditorGUILayout.LabelField("Random Settings", EditorStyles.boldLabel);
            grid.gridFill = EditorGUILayout.Slider(new GUIContent("Grid Fill", "How much cells compare to total grid cells should be filled ?"), grid.gridFill, 0.0f, 1.0f);
    }

    private void islandGUI()
    {
        int maxIslandSize = (grid.radius * grid.radius) / grid.islandsNumber;
        int maxIslandNumber = (grid.radius * grid.radius) / grid.islandMaxSize;
        EditorGUILayout.LabelField("Islands Settings", EditorStyles.boldLabel);
            grid.islandMinSize = EditorGUILayout.IntSlider(new GUIContent("Island Min Size", "Explicit"), grid.islandMinSize, 1, maxIslandSize);
            grid.islandMaxSize = EditorGUILayout.IntSlider(new GUIContent("Island Max Size", "Explicit"), grid.islandMaxSize, grid.islandMinSize, maxIslandSize);
            grid.islandsNumber = EditorGUILayout.IntSlider(new GUIContent("Number of Islands", "Explicit"), grid.islandsNumber, 1, maxIslandNumber);
    }
}
