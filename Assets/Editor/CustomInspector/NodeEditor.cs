using UnityEditor;
using UnityEngine;

public class WaypointEditor : EditorWindow
{
    private Node FirstNode;
    private Node SecondNode;

    [MenuItem("Game/Waypoints")]
    public static void ShowWindow()
    {
        GetWindow<WaypointEditor>(false, "Waypoints", true);
    }

    void OnGUI()
    {
        FirstNode = (Node)EditorGUILayout.ObjectField(new GUIContent("First node"), FirstNode, typeof(Node), true);
        SecondNode = (Node)EditorGUILayout.ObjectField(new GUIContent("Second node"), SecondNode, typeof(Node), true);

        if (FirstNode != null)
        {
            SerializedProperty f_connections = new SerializedObject(FirstNode).FindProperty("connections");
            DisplayConnections(f_connections, FirstNode);
        }

        if (SecondNode != null)
        {
            SerializedProperty f_connections = new SerializedObject(SecondNode).FindProperty("connections");
            DisplayConnections(f_connections, SecondNode);
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create link"))
            {
                SerializedProperty f_connections = new SerializedObject(FirstNode).FindProperty("connections");
                AddIfNotPresent(f_connections, SecondNode);

                SerializedProperty s_connections = new SerializedObject(SecondNode).FindProperty("connections");
                AddIfNotPresent(s_connections, FirstNode);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void OnDrawGizmos() // ???? not working
    {
        SerializedProperty f_connections = new SerializedObject(FirstNode).FindProperty("connections");
        DisplayConnections(f_connections, FirstNode);

        SerializedProperty s_connections = new SerializedObject(SecondNode).FindProperty("connections");
        DisplayConnections(s_connections, SecondNode);
    }

    private void AddIfNotPresent(SerializedProperty list, Object elem)
    {
        int i = 0;
        while (i < list.arraySize && !list.GetArrayElementAtIndex(i).objectReferenceValue.Equals(elem)) ++i;
        if (i == list.arraySize)
        {
            list.arraySize++;
            list.GetArrayElementAtIndex(i).objectReferenceValue = elem;
        }
        list.serializedObject.ApplyModifiedProperties();
    }

    private void DisplayConnections(SerializedProperty neighbours, Node node)
    {
        for (int i = 0; i < neighbours.arraySize; ++i)
        {
            Node other = (Node)neighbours.GetArrayElementAtIndex(i).objectReferenceValue;
            Debug.DrawLine(node.transform.position, other.transform.position);
        }
    }
}

[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class NodeEditor : Editor
{
    public void OnSceneGUI()
    {
        Node node = target as Node;
        for (int i = 0; i < node.connections.Count; ++i)
        {
            Handles.DrawLine(node.transform.position, node.connections[i].transform.position);
        }
    }
}